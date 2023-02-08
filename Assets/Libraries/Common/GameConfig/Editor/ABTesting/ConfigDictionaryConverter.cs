#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace _Game.Editor.Utils
{
    public class ConfigDictionaryConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            GenericConfig? genericConfig = value as GenericConfig;
            if (genericConfig == null)
                return;
            
            Dictionary<string, object> configDict = new Dictionary<string, object>();
            configDict.Add("name", genericConfig.name);
            configDict.Add("structure", genericConfig.structure);
            
            List<object> itemsList = new List<object>();
            configDict.Add("items", itemsList);
            
            foreach (var configItem in genericConfig.items)
            {
                Dictionary<string, object?> currentItemDict = new Dictionary<string, object?>();
                foreach (var itemKey in configItem.Keys)
                {
                    if (string.IsNullOrEmpty(configItem[itemKey]))
                        continue;
                    
                    string typeString = genericConfig.structure[itemKey].Split(',')[0].RemoveInvisible();
                    object? itemFieldValue = null;

                    itemFieldValue = GetItemFieldValue(typeString, configItem, itemKey);
                    
                    currentItemDict.Add(itemKey, itemFieldValue);
                }
                itemsList.Add(currentItemDict);
            }
            
            JObject jObject = JObject.FromObject(configDict);
            jObject.WriteTo(writer);
        }

        private static object? GetItemFieldValue(string typeString,  ConfigSerializedDictionary configItem,
            string itemKey)
        {
            object? itemFieldValue = null;
            
            if (typeString == "string")
            {
                itemFieldValue = configItem[itemKey];
            }
            else if (typeString == "string[]")
            {
                itemFieldValue = JsonConvert.DeserializeObject<string[]>(configItem[itemKey]);
            }
            else if (typeString == "bool")
            {
                itemFieldValue = bool.Parse(configItem[itemKey]);
            }
            else if (typeString == "bool[]")
            {
                itemFieldValue = JsonConvert.DeserializeObject<bool[]>(configItem[itemKey]);
            }
            else if (typeString == "int")
            {
                itemFieldValue = int.Parse(configItem[itemKey]);
            }
            else if (typeString == "int[]")
            {
                itemFieldValue = JsonConvert.DeserializeObject<int[]>(configItem[itemKey]);
            }
            else if (typeString == "float")
            {
                itemFieldValue = float.Parse(configItem[itemKey], CultureInfo.InvariantCulture);
            }
            else if (typeString == "float[]")
            {
                itemFieldValue = JsonConvert.DeserializeObject<float[]>(configItem[itemKey]);
            }

            return itemFieldValue;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GenericConfig);
        }
    }
}