using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Editor.Utils
{
    [Serializable]
    public class AbTestSuffixes
    {
        public string name;
        [HideInInspector] public ConfigSerializedDictionary structure;
        public List<AbTestCase> items;

        public void TryAddCase(string id, string suffix)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var testCase = items[i];
                if (testCase.id.Equals(id) && testCase.suffix.Equals(suffix))
                {
                    return;
                }

                if (testCase.id.Equals(id) || testCase.suffix.Equals(suffix))
                {
                    Debug.LogErrorFormat(
                        $"Seems like duplicated id or suffix and will be replaced with the remote ones. id: {id} suffix:{suffix}.");
                    items.RemoveAt(i);
                    break;
                }
            }

            items.Add(new AbTestCase
            {
                id = id,
                suffix = suffix
            });
        }
    }

    [Serializable]
    public class AbTestCase
    {
        public string id;
        public string suffix;
    }
    
}