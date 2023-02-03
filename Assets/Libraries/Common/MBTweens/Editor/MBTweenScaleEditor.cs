using UnityEngine;
 using UnityEditor;

namespace MBTweens
{
 [CustomEditor(typeof(MBTweenScale))]
 public class MBTweenScaleEditor : MBTweenBaseEditor
 {

      public override void OnInspectorGUI()
      {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Set Begin From Target"))
            {
                MBTweenScale tp = tween as MBTweenScale;
                if (tp != null)
                {
                  tp.startScale = tp.Target.localScale;
                }
            }
			         
            if (GUILayout.Button("Set End From Target"))
            {
                MBTweenScale tp = tween as MBTweenScale;
                 if (tp != null)
                 {
                   tp.endScale = tp.Target.localScale;
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