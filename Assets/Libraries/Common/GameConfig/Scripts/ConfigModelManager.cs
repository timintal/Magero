using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using StopWatch = System.Diagnostics.Stopwatch;
#endif
using System.Runtime.CompilerServices;
using System;


namespace Game.Config.Model
{
    public static class ExtensionMethods
    {
        public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += obj => { tcs.SetResult(null); };
            return ((Task)tcs.Task).GetAwaiter();
        }
    }

    public class GameConfig
    {
        private Action OnComplete;

        private bool Initialised { get; set; }
        private ConfigData _data;

        public Dictionary<string, T> GetConfigModel<T>() where T : IConfigModel
        {
            return ModelProcessor.GetConfigModel<T>();
        }

        public async void Init(Action onComplete, List<string> sheetsToLoad = null)
        {
            OnComplete = onComplete;
            _data = Resources.Load("GameConfig/ConfigData") as ConfigData;

            var allSheets = _data.GetDefaultSheets();
            if (sheetsToLoad != null)
            {
                allSheets.AddRange(sheetsToLoad);
            }
            for (var i = 0; i < allSheets.Count; i++)
            {
                allSheets[i] = allSheets[i] + ".json";
            }

            var sheets = allSheets.ToArray();
            await UpdateData(sheets);
            LoadTheModel(sheets);
        }


        public static async Task<string> GetURLHelper(string url)
        {
            var req = UnityWebRequest.Get(url);
            req.timeout = 3;
            var res = req.SendWebRequest();
            await res;

#if UNITY_2020_1_OR_NEWER
            bool success = res.webRequest.result == UnityWebRequest.Result.Success;
            string error = res.webRequest.result.ToString();
#else
            bool success = !res.webRequest.isNetworkError && !res.webRequest.isHttpError;
            string error = $"Network = {res.webRequest.isNetworkError} http = {res.webRequest.isHttpError}";
#endif
            if (success)
            {
                return res.webRequest.downloadHandler.text;
            }
            else
            {
                Debug.LogWarning("URL error: url=" + url + " error:" + error);
            }
            return "";
        }

        private async Task UpdateData(string[] sheetsToLoad)
        {
            // in the first ever run we will cache the data if it doesn't exist on the drive.
            EnsureCachedDataExists(sheetsToLoad);

            return;
            
            bool checkCDN = true;
            bool reDownloadFromCDNEvenIfSame = false;

#if UNITY_EDITOR
            bool areWeValidatingDuringImport = !Application.isPlaying;
            if (_data.Debug_UseLocalConfigModel || areWeValidatingDuringImport)
            {
                CopyLocalDataToCache(sheetsToLoad);
                checkCDN = false;
            }

            if (!areWeValidatingDuringImport)
            {
                // The adminData.Debug_AlwaysDownloadFromCDN is there so we can simulate worst case, otherwise this will run 
                // with different timing as all of the operations will be local and fast. 
                // We don't want to do this if we are calling validate during an import (as we want it to run as fast as possible).
                reDownloadFromCDNEvenIfSame = _data.Debug_AlwaysDownloadFromCDN;
            }
#endif
            if (checkCDN || reDownloadFromCDNEvenIfSame)
            {
                var localHashes = new Dictionary<string, string>();
                // first read the local index to see what we have.
                if (File.Exists(_data.GetCachedIndexFile()))
                {
                    var input = File.ReadAllText(_data.GetCachedIndexFile());
                    localHashes = JsonConvert.DeserializeObject<Dictionary<string, string>>(input);
                }

                // read the one from the cloud
                var cloudIndexContent = await GetURLHelper(_data.GetS3IndexFile());
                var cloudHashes = JsonConvert.DeserializeObject<Dictionary<string, string>>(cloudIndexContent);

                if (cloudHashes == null)
                {
                    Debug.LogWarning(
                        $"No S3 Bucket defined or broken index file.  Checked [url={_data.GetS3Path()}] Using only local settings!");
                }
                else
                {
                    var sheetLoadTasks = new List<Task>();
                    foreach (var sheet in sheetsToLoad)
                    {
                        string fileName = Path.Combine(_data.GetCachedPath(), sheet);

                        // if we don't have it, get it. 
                        bool getNewVersion = !File.Exists(fileName);

                        if (!getNewVersion)
                        {
                            // see if the hash is ok
                            string sheetName = sheet.Replace(".json", "");
                            // string localHash;
                            localHashes.TryGetValue(sheetName, out string localHash);
                            getNewVersion = localHash != cloudHashes[sheetName];
                        }

                        if (getNewVersion || reDownloadFromCDNEvenIfSame)
                        {
                            var url = _data.GetS3Path() + sheet;
                            sheetLoadTasks.Add(GetSheetData(url, fileName));
                        }
                    }

                    await Task.WhenAll(sheetLoadTasks);

                    // write our new index file (do this last so if something fails we will try again next launch)
                    File.WriteAllText(_data.GetCachedIndexFile(), cloudIndexContent);
                }
            }
        }

        private async Task GetSheetData(string url, string fileName)
        {
            var json = await GetURLHelper(url);
            if (!string.IsNullOrEmpty(json))
            {
                File.WriteAllText(fileName, json);
            }
        }

        private void LoadTheModel(string[] sheetsToLoad)
        {
            // load the files, always load the files. 
#if UNITY_EDITOR
            var watch = StopWatch.StartNew();
#endif
            ModelProcessor.InitConfigModelData();
            var error = "";
            foreach (var sheet in sheetsToLoad)
            {
                var configModelData = GetModelDataFromCache(sheet);
                error = ModelProcessor.ProcessConfigModelData(configModelData);
                if (ShowError(error)) 
                    return;
            }
            error = ModelProcessor.ValidateModelReferences();
            if (ShowError(error)) 
                return;
            
#if UNITY_EDITOR
            Debug.Log($"[ConfigModelManager.ProcessConfigModel] config model processed in {watch.ElapsedMilliseconds}ms");
            watch.Stop();
#endif
            Initialised = string.IsNullOrEmpty(error);
            OnComplete?.Invoke();
        }

        private static bool ShowError(string error)
        {
            if (string.IsNullOrEmpty(error)) 
                return false;
            
            Debug.LogWarning(error);
            
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Error", error, "ok");
#endif         
            return true;
        }

        private string GetModelDataFromCache(string file)
        {
            string inFile = Path.Combine(_data.GetCachedPath(), file);
            if (File.Exists(inFile))
            {
                return File.ReadAllText(inFile);
            }
            return "";
        }

        public void EnsureCachedDataExists(string[] sheetsToLoad)
        {
            if (!Directory.Exists(_data.GetCachedPath()))
            {
                Directory.CreateDirectory(_data.GetCachedPath());
                CopyLocalDataToCache(sheetsToLoad);
            }
        }

        public void CopyLocalDataToCache(string[] sheetsToLoad)
        {
            foreach (var sheet in sheetsToLoad)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                var loadingRequest = UnityWebRequest.Get(Path.Combine(RemoteConfigModelPaths.LocalAssetPath, sheet));
                loadingRequest.SendWebRequest();
                while (!loadingRequest.isDone) {
                    if (loadingRequest.isNetworkError || loadingRequest.isHttpError) {
                        break;
                    }
                }
                if (loadingRequest.isNetworkError || loadingRequest.isHttpError) {
 
                } else {
                    File.WriteAllBytes(Path.Combine(_adminData.GetCachedPath(), sheet), loadingRequest.downloadHandler.data);
                }
#else
                File.Copy(Path.Combine(ConfigPaths.LocalAssetPath, sheet), Path.Combine(_data.GetCachedPath(), sheet), true);
#endif
            }

#if UNITY_EDITOR
            // also copy the index file here, we don't need to worry about it for iOS/Android as this is only used in the editor testing
            File.Copy(ConfigPaths.LocalIndexFile, _data.GetCachedIndexFile(), true);
#endif
        }

    }
}