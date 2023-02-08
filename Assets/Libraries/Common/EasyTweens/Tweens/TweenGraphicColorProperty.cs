using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace EasyTweens
{
    [Serializable]
    public class TweenGraphicColorProperty : ColorTween<Graphic>
    {
        public static string TweenName => "UI Graphic Color";

        protected override Color Property
        {
            get => target.color;
            set
            {
                target.color = value;
                #if UNITY_EDITOR
                
                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(target);
                }
                #endif
            }
        }
    }
}