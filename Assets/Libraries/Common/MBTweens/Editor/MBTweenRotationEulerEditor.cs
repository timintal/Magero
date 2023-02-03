using UnityEngine;
using UnityEditor;

namespace MBTweens
{
    [CustomEditor(typeof(MBTweenRotationEulers))]
    public class MBTweenRotationEulerEditor : MBTweenBaseEditor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Set Begin From Target"))
            {
                MBTweenRotationEulers tp = tween as MBTweenRotationEulers;
                if (tp != null)
                {
                    tp.StartRotation = tp.Target.localRotation.eulerAngles;
                }
            }
			
            if (GUILayout.Button("Set End From Target"))
            {
                MBTweenRotationEulers tp = tween as MBTweenRotationEulers;
                if (tp != null)
                {
                    tp.EndRotation = tp.Target.localRotation.eulerAngles;
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