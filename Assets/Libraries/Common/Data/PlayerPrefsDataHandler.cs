using System;
using Newtonsoft.Json;
using UnityEngine;

namespace _Game.Data
{
    public class PlayerPrefsDataHandler : IPersistentDataHandler
    {
        public void Save(PersistentDataBase data)
        {
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(data.DataId, json);
            PlayerPrefs.Save();
            data.IsDirty = false;
        }

        public void Load<T>(T data) where T : PersistentDataBase
        {
            if (PlayerPrefs.HasKey(data.DataId))
            {
                string json = PlayerPrefs.GetString(data.DataId);
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace
                    };
                    JsonConvert.PopulateObject(json, data, settings);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Failed to deserialize {data.DataId}: {exception.Message})");
                }
            }
        }
    }
}