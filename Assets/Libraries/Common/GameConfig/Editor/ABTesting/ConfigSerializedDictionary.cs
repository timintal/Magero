using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigSerializedDictionary : Dictionary<string, string>, ISerializationCallbackReceiver
{
    //------------------------------------------------------------------------------------------------------------------
    [SerializeField, HideInInspector]
    private List<string> keyData = new List<string>();

    [SerializeField, HideInInspector] private List<string> valueData = new List<string>();
    
    public string ItemId
    {
        get
        {
            for (var i = 0; i < keyData.Count; i++)
            {
                string currKey = keyData[i];
                if (currKey.Equals("id"))
                {
                    return valueData[i];
                }
            }

            return string.Empty;
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        this.Clear();
        for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
        {
            this[this.keyData[i]] = this.valueData[i];
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        this.keyData.Clear();
        this.valueData.Clear();

        foreach (var item in this)
        {
            this.keyData.Add(item.Key);
            this.valueData.Add(item.Value);
        }
    }

    public override bool Equals(object other)
    {
        ConfigSerializedDictionary otherDict = other as ConfigSerializedDictionary;
        if (otherDict == null)
        {
            return false;
        }

        if (otherDict.keyData.Count != this.keyData.Count ||
            otherDict.valueData.Count != this.valueData.Count)
        {
            return false;
        }

        for (int i = 0; i < keyData.Count; i++)
        {
            if (!keyData[i].Equals(otherDict.keyData[i]) ||
                !valueData[i].Equals(otherDict.valueData[i]))
            {
                return false;
            }
        }

        return true;
    }

    public ConfigSerializedDictionary Clone()
    {
        ConfigSerializedDictionary dict = new ConfigSerializedDictionary();

        for (int i = 0; i < keyData.Count; i++)
        {
            dict.Add(keyData[i], valueData[i]);
        }

        return dict;
    }

}