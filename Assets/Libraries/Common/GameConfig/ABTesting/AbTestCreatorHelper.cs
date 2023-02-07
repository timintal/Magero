using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Editor.Utils
{
    [CreateAssetMenu(menuName = "Utils/ABTestCreatorHelper")]
    public class AbTestCreatorHelper : ScriptableObject
    {
        private const string FULL_CONFIG = "FULL CONFIG";
        private const string REPORT = "REPORT";
        private const string SUFFIXES = "SUFFIXES";

        [TabGroup(FULL_CONFIG)] public GameConfigSet fullConfig;
        
        [TabGroup(SUFFIXES)] public AbTestSuffixes[] currentSuffixes;

        [TabGroup(FULL_CONFIG)] [FolderPath(AbsolutePath = true)]
        public string configPath;

        [TabGroup(REPORT)] [ListDrawerSettings(ListElementLabelName = "Suffix")]
        public List<AbTestModification> testModifications;


        [TabGroup(FULL_CONFIG)]
        [Button]
        void LoadFullConfig()
        {
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = Path.Combine(Application.dataPath, "StreamingAssets/RemoteConfigModel/Data");
            }
            
            fullConfig.gameConfig.Clear();
            foreach (var file in Directory.EnumerateFiles(configPath))
            {
                if (Path.GetExtension(file) != ".json" ||
                    Path.GetFileName(file) == "index.json" ||
                    Path.GetFileName(file) == "AbTestSuffixes.json")
                    continue;

                string json = File.ReadAllText(file);
                List<GenericConfig> configs = ParseConfig(json);
                fullConfig.gameConfig.Add(new ConfigFile {Configs = configs, FileName = file});
            }
        }

        [TabGroup(FULL_CONFIG)]
        [Button]
        void SaveFullConfig()
        {
            foreach (var configFile in fullConfig.gameConfig)
            {
                File.WriteAllText(configFile.FileName,
                    JsonConvert.SerializeObject(configFile.Configs, new ConfigDictionaryConverter()));
            }
            ModifyIndexFile();
        }

        private static List<GenericConfig> ParseConfig(string json)
        {
            JArray jArray = JArray.Parse(json);
            List<GenericConfig> parsedConfig = new List<GenericConfig>();

            for (int i = 0; i < jArray.Count; i++)
            {
                GenericConfig config = new GenericConfig();

                JToken jName = jArray[i]["name"];

                config.name = jName.ToString();

                JToken jStructure = jArray[i]["structure"];
                var structure = jStructure.ToObject<ConfigSerializedDictionary>();

                config.structure = structure;

                config.items = new List<ConfigSerializedDictionary>();
                List<ConfigSerializedDictionary> referenceItems = new List<ConfigSerializedDictionary>();
                JArray jItems = jArray[i]["items"] as JArray;
                foreach (var item in jItems)
                {
                    ConfigSerializedDictionary dict = new ConfigSerializedDictionary();
                    ConfigSerializedDictionary referenceDict = new ConfigSerializedDictionary();

                    foreach (var key in structure.Keys)
                    {
                        dict.Add(key, item[key] != null ? item[key].ToString().RemoveInvisible() : String.Empty);
                        referenceDict.Add(key,
                            item[key] != null ? item[key].ToString().RemoveInvisible() : String.Empty);
                    }

                    config.items.Add(dict);
                    referenceItems.Add(referenceDict);
                }

                config.ReferenceItems = referenceItems;
                parsedConfig.Add(config);
            }

            return parsedConfig;
        }

        void SaveConfigJson(string fileName, string json)
        {
            var filePath = Path.Combine(configPath, fileName);
            File.WriteAllText(filePath, json);
        }

        [TabGroup(SUFFIXES)]
        [Button]
        void AddSuffixesForAbTestWithId(string testID)
        {
            if (LoadSuffixes())
            {
                PlayerPrefs.SetInt("RemoteConfigVersion", 1);
                // TODO: Went wrong when going from VS 6.0 to 6.1.2
                //VoodooTuneManager.GetConfigurationHierarchy(root => FillSuffixes(root, testID));
            }
        }

        [TabGroup(SUFFIXES)]
        [Button]
        private bool LoadSuffixes()
        {
            var suffixesJson = GetJson("AbTestSuffixes.json");

            currentSuffixes = JsonConvert.DeserializeObject<AbTestSuffixes[]>(suffixesJson);

            if (currentSuffixes == null || currentSuffixes.Length == 0)
            {
                Debug.LogError("Suffixes JSON have not been found!");
                return false;
            }

            return true;
        }

        private string GetJson(string path)
        {
            foreach (var file in Directory.EnumerateFiles(configPath))
            {
                if (Path.GetFileName(file) == path)
                {
                    return File.ReadAllText(file);
                }
            }

            return String.Empty;
        }

        [TabGroup(SUFFIXES)]
        [Button]
        void SaveCurrentSuffixes()
        {
            SaveConfigJson("AbTestSuffixes.json", JsonConvert.SerializeObject(currentSuffixes));
        }

        [TabGroup(REPORT)]
        [Button]
        void GenerateReport()
        {
            if (currentSuffixes == null || currentSuffixes.Length == 0)
            {
                LoadSuffixes();
            }

            testModifications.Clear();

            foreach (var suffixRoot in currentSuffixes)
            {
                foreach (var testCase in suffixRoot.items)
                {
                    CollectModificationsForSuffix(testCase.suffix);
                }
            }
        }

        private void CollectModificationsForSuffix(string suffix)
        {
            var modification = new AbTestModification
                {Suffix = suffix, Modifications = new ModificationDictionary()};

            foreach (var configFile in fullConfig.gameConfig)
            {
                foreach (var config in configFile.Configs)
                {
                    foreach (var item in config.items)
                    {
                        if (item.ItemId.Contains(suffix))
                        {
                            AddModification(suffix, item, config, modification);
                        }
                    }
                }
            }

            if (modification.Modifications.Count > 0)
            {
                testModifications.Add(modification);
            }
        }

        private static void AddModification(string suffix, ConfigSerializedDictionary item, GenericConfig config,
            AbTestModification modification)
        {
            string originalId = item.ItemId.Replace(suffix, "");
            var originalItem = config.items.Find(cfg => cfg.ItemId == originalId);

            if (originalItem != null)
            {
                ConfigSerializedDictionary origDictionary = new ConfigSerializedDictionary();
                ConfigSerializedDictionary modifiedDictionary = new ConfigSerializedDictionary();

                foreach (var pair in item)
                {
                    if (!originalItem[pair.Key].Equals(pair.Value) && pair.Key != "id")
                    {
                        origDictionary.Add(pair.Key, originalItem[pair.Key]);
                        modifiedDictionary.Add(pair.Key, pair.Value);
                    }
                }

                modification.Modifications.Add(config.name, new TestModification
                {
                    Original = origDictionary,
                    Modified = modifiedDictionary
                });
            }
        }
        
        private void ModifyIndexFile()
        {
            foreach (var file in Directory.EnumerateFiles(configPath))
            {
                if (Path.GetFileName(file) == "index.json")
                {
                    string json = File.ReadAllText(file);
                    Dictionary<string,string> indexContent = 
                        JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
                    Dictionary<string, string> modifiedContent = new Dictionary<string, string>();

                    foreach (var sheetHashPair in indexContent)
                    {
                        string hash = sheetHashPair.Value;

                        var numbers = hash.Split('-');

                        int randomIndex = Random.Range(0, numbers.Length);
                        
                        int number = int.Parse(numbers[randomIndex], NumberStyles.HexNumber);
                        int randomNumber = number;
                        while (number == randomNumber)
                        {
                            randomNumber = Random.Range(0, 255);
                        }
                        
                        numbers[randomIndex] = randomNumber.ToString("X2");
                        modifiedContent[sheetHashPair.Key] = string.Join("-", numbers);
                    }

                    File.WriteAllText(file, JsonConvert.SerializeObject(modifiedContent));
                    
                    break;
                }
            }
        }

        [Button(ButtonSizes.Large)]
        [GUIColor(1f, 0.3f, 0.3f)]
        void Clear()
        {
            fullConfig.gameConfig.Clear();
            currentSuffixes = null;
            testModifications.Clear();
        }
    }

    public static class StringExtensions
    {
        public static string RemoveInvisible(this string input)
        {
            return new string(input.Where(c => !char.IsControl(c)).ToArray());
        }
    }
}