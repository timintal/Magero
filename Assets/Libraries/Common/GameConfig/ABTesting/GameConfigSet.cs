using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace _Game.Editor.Utils
{
    [Serializable]
    public class GameConfigSet
    {
        [ListDrawerSettings(OnBeginListElementGUI = nameof(BeginDraw), OnEndListElementGUI = nameof(EndDraw))]
        public List<ConfigFile> gameConfig;
        
        void BeginDraw(int index)
        {
            bool modified = gameConfig[index].IsAnythingModified;
            GUIHelper.PushColor(modified ? Color.yellow : Color.white);
            SirenixEditorGUI.Title(gameConfig[index].ShortName + (modified ? "*" : ""), String.Empty, TextAlignment.Left, false, modified);
            GUIHelper.PopColor();
        }

        void EndDraw(int index)
        {
            
        }
    }

    [Serializable]
    public class ConfigFile
    {
        [HideInInspector] public string FileName;
        
        public List<GenericConfig> Configs;

        public string ShortName => Path.GetFileName(FileName);

        public bool IsAnythingModified => Configs.Any(config => config.IsModified);
       
    }
    
}