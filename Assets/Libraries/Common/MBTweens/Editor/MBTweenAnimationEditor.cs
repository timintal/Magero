using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MBTweens
{
	[CustomEditor(typeof(MBTweenAnimation))]
	public class MBTweenAnimationEditor : Editor
	{
		private bool testInEditor;
		private bool subscribed;

		private MBTweenAnimation anim;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			anim = target as MBTweenAnimation;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Animation Duration Scale");
			float prev = anim.DurationScale;
			float newDurScale = EditorGUILayout.FloatField(anim.DurationScale);
			if (!Mathf.Approximately(prev, newDurScale))
			{
				anim.DurationScale = newDurScale;
				EditorUtility.SetDirty(anim);
			}
			EditorGUILayout.EndHorizontal();
			DrawTweenList();

			DrawDebugButtons(anim);

			if (!subscribed && testInEditor)
			{
				subscribed = true;
				anim.SubscribeToEditorUpdates();
			}
			else if (subscribed && !testInEditor)
			{
				subscribed = false;
				anim.UnsubscribeFromEditorUpdates();
			}
			
			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
			}
		}

		private bool expand = true;

		void DrawTweenList()
		{
			if (anim == null)
			{
				return;
			}

			Color oldCOntentColor = GUI.contentColor;
			GUI.contentColor = Color.blue;
			expand = EditorGUILayout.Foldout(expand, "NewTweensEditor");

			GUI.contentColor = oldCOntentColor;

			if (!expand)
				return;

			GUIStyle headerStyle = new GUIStyle();
			headerStyle.fontSize = 30;
			headerStyle.fontStyle = FontStyle.Bold;
			headerStyle.alignment = TextAnchor.MiddleCenter;

			GUIStyle delayStyle = new GUIStyle(GUI.skin.textField);
			delayStyle.alignment = TextAnchor.MiddleRight;


			EditorGUILayout.BeginHorizontal(GUILayout.Height(40));
			EditorGUILayout.LabelField("Tweens:", headerStyle);
			EditorGUILayout.EndHorizontal();

			headerStyle.fontSize = 12;
			headerStyle.alignment = TextAnchor.MiddleLeft;
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			EditorGUILayout.LabelField("", headerStyle, GUILayout.Width(25));
			EditorGUILayout.LabelField("TweenType", headerStyle, GUILayout.Width(100));
			EditorGUILayout.LabelField("Reference", headerStyle, GUILayout.Width(150));
			EditorGUILayout.LabelField("Delay", headerStyle, GUILayout.Width(50));
			EditorGUILayout.LabelField("IsFwd", headerStyle, GUILayout.Width(60));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			Color oldColor = GUI.color;

			for (int i = 0; i < anim.Tweens.Count; i++)
			{
				MBTweenAnimation.TweenEntry entry = anim.Tweens[i];

				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();

				GUI.color = Color.red;
				if (GUILayout.Button("X", GUILayout.Width(25)))
				{
					anim.Tweens.RemoveAt(i);
					break;
				}
				
				GUI.color = oldColor;
				string name = entry.tween != null ? entry.tween.GetType().ToString().Split('.').Last().Replace("MBTween", "") : "Unknown";

				EditorGUILayout.LabelField(name, new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold},GUILayout.Width(100));
				entry.tween =
					EditorGUILayout.ObjectField(entry.tween, typeof(MBTweenBase), true, GUILayout.Width(150)) as
						MBTweenBase;
				entry.delay = EditorGUILayout.FloatField(entry.delay, delayStyle, GUILayout.Width(50));

				EditorGUILayout.BeginHorizontal(GUILayout.Width(60));
				GUILayout.FlexibleSpace();
				entry.isForwardOnlyTween = EditorGUILayout.Toggle(entry.isForwardOnlyTween, GUILayout.Width(10));
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				GUI.color = oldColor;

				GUILayout.Space(10);

				anim.Tweens[i] = entry;

				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}

			GUI.color = Color.green;

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add New Tween"))
			{
				anim.Tweens.Add(new MBTweenAnimation.TweenEntry());
			}

			EditorGUILayout.EndHorizontal();

			GUI.color = oldColor;

		}

		private void OnDestroy()
		{
			if (anim != null)
			{
				anim.UnsubscribeFromEditorUpdates();
			}
		}

		private void OnDisable()
		{
			if (subscribed && anim != null)
			{
				subscribed = false;
				anim.UnsubscribeFromEditorUpdates();
			}

			testInEditor = false;
		}

		private void OnEnable()
		{
			testInEditor = true;
		}

		static void DrawDebugButtons(MBTweenAnimation animation)
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Set End State"))
			{
				animation.SetEndState(0);
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Set Begin State"))
			{
				animation.SetBeginState(0);
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			Color oldColor = GUI.color;
			GUI.color = Color.cyan;
			if (GUILayout.Button("Set child Tweens"))
			{
				Undo.RecordObject(animation, "MBTweenAnimation child fill");
				EditorUtility.SetDirty(animation);
				animation.Tweens.Clear();
				MBTweenBase[] ts = animation.GetComponentsInChildren<MBTweenBase>();
				foreach (var tweenBase in ts)
				{
					MBTweenAnimation.TweenEntry te = new MBTweenAnimation.TweenEntry {tween = tweenBase};
					te.delay = te.tween.delay;
					animation.Tweens.Add(te);
				}
			}

			EditorGUILayout.EndHorizontal();
			GUI.color = oldColor;
		}

	}
}