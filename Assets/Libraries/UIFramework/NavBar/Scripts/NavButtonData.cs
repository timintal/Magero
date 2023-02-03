using System;
using UnityEngine;

namespace Magero.UIFramework.Components.NavBar
{
    [Serializable]
    public class NavButtonData
    {
        public string name;
        public Sprite icon = null;
        public NavBarScreen prefab;
    }
}