using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Editor.Utils
{
    [Serializable]
    public class ModificationDictionary : UnitySerializedDictionary<string, TestModification> {}

    [Serializable]
    public class AbTestModification
    {
        [HideInInspector] public string Suffix;
        public ModificationDictionary Modifications;
    }

    [Serializable]
    public class TestModification
    {
        public ConfigSerializedDictionary Original;
        public ConfigSerializedDictionary Modified;
    }
}