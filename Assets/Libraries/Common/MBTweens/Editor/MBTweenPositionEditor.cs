using UnityEngine;
using UnityEditor;

namespace MBTweens
{
	[CustomEditor(typeof(MBTweenPosition))]
	public class MBTweenPositionEditor : MBTweenBaseEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Set Begin From Target"))
			{
				MBTweenPosition tp = tween as MBTweenPosition;
				if (tp != null)
				{
					tp.StartPosition = tp.Target.localPosition;
				}
			}
			
			if (GUILayout.Button("Set End From Target"))
			{
				MBTweenPosition tp = tween as MBTweenPosition;
				if (tp != null)
				{
					tp.EndPosition = tp.Target.localPosition;
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