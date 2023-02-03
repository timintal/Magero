using UnityEngine;
using UnityEditor;

namespace MBTweens
{
    [CustomEditor(typeof(MBTweenColor))]
    public class MBTweenColorEditor : MBTweenBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Init Target"))
            {
                MBTweenColor tp = tween as MBTweenColor;
                if (tp != null)
                {
                    tp.InitTarget();
                }
            }

            EditorGUILayout.EndHorizontal();
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
