using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace _Game.Editor.Utils
{
    [Serializable]
    public class GenericConfig
    {
        public string name;
        [HideInInspector] public ConfigSerializedDictionary structure;
        
        [ListDrawerSettings(ListElementLabelName = "ItemId", ShowIndexLabels = false,
            OnBeginListElementGUI = "BeginDraw", OnEndListElementGUI = "EndDraw", OnTitleBarGUI = "Title")]
        public List<ConfigSerializedDictionary> items;

        [HideInInspector] [JsonIgnore] public List<ConfigSerializedDictionary> ReferenceItems;

        [JsonIgnore] public bool IsModified
        {
            get
            {
                if (ReferenceItems.Count != items.Count)
                {
                    return true;
                }

                for (var i = 0; i < ReferenceItems.Count; i++)
                {
                    if (ReferenceItems[i].Equals(items[i]))
                    {
                        return false;
                    }
                }

                return false;
            }
        }
        
        void Title()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Rotate))
            {
                try
                {
                    ResetConfig();
                }
                catch 
                {
                    //I don't care that it will be index out of range 🤪
                }
            }

            if (IsModified)
            {
                GUIHelper.PushColor(Color.yellow);
                SirenixEditorGUI.MessageBox("*Modified");
                GUIHelper.PopColor();
            }
        }
        
        void BeginDraw(int index)
        {
            string statusString = string.Empty;
            Color color = Color.white;
            
            string key = items[index].ItemId;
            
            if (items.Count(config => config.ItemId.Equals(key)) > 1)
            {
                statusString = " - Duplicated Key";
                color = new Color(0.9f, 0.6f, 0.6f);
            }
            else
            {
                if (ReferenceItems != null)
                {
                    ConfigSerializedDictionary reference = ReferenceItems.Find(config => config.ItemId.Equals(key));

                    if (reference == null)
                    {
                        statusString = " - Added";
                        color = new Color(0.6f, 0.9f, 0.6f);
                    }
                    else if (!items[index].Equals(reference))
                    {
                        statusString = " - Modified";
                        color = new Color(0.7f, 0.7f, 0.9f);
                    }
                }
            }

            GUIHelper.PushColor(color);

            SirenixEditorGUI.BeginBox();
            Rect headerRect = SirenixEditorGUI.BeginBoxHeader();
            SirenixEditorGUI.Title($"{items[index].ItemId} {statusString}", 
                string.Empty,
                TextAlignment.Left,
                true,
                !string.IsNullOrEmpty(statusString));
            
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Pen))
            {
                items.Add(items[index].Clone());
            }
            SirenixEditorGUI.EndBoxHeader();
            GUIHelper.PopColor();
        }

        void EndDraw(int index)
        {
            SirenixEditorGUI.EndBox();
            
        }

        ConfigSerializedDictionary GetItemByID(string id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ItemId.Equals(id))
                {
                    return items[i];
                }
            }

            return null;
        }

        public void ResetConfig()
        {
            if (ReferenceItems != null)
            {
                items.Clear();
                for (int i = 0; i < ReferenceItems.Count; i++)
                {
                    ConfigSerializedDictionary dict = ReferenceItems[i];
                    ConfigSerializedDictionary newDict = new ConfigSerializedDictionary();
                    
                    foreach (var pair in dict)
                    {
                        newDict.Add(pair.Key, pair.Value);                           
                    }
                    items.Add(newDict);
                }
            }
        }
    }
}