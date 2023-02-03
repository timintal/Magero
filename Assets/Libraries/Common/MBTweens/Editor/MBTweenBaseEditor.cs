using System;
using UnityEngine;
using UnityEditor;

namespace MBTweens
{
    [CustomEditor(typeof(MBTweenBase), true)]
    public class MBTweenBaseEditor : Editor
    {
        private bool testInEditor;
        private bool subscribed;

        protected MBTweenBase tween;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("General Parameters:", EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            if (tween == null)
            {
                tween = (MBTweenBase) target;
                tween.enabled = false;
            }

            tween.playOnAwake = EditorGUILayout.Toggle("Play On Awake", tween.playOnAwake);
            
            tween.duration = EditorGUILayout.FloatField("Duration", tween.duration);
            tween.durationScale = EditorGUILayout.FloatField("Duration Scale", tween.durationScale);
            tween.smooth = EditorGUILayout.Toggle("Smooth", tween.smooth);
            
            tween.delay = EditorGUILayout.FloatField(new GUIContent("Delay", "Can be overriden by MBTweenAnimation"),
                tween.delay);

            tween.ignoreTimeScale = EditorGUILayout.Toggle("Ignore Time Scale", tween.ignoreTimeScale);

            tween.easingMethod = (EasingMethod) EditorGUILayout.EnumPopup("Easing Method", tween.easingMethod);

            if (tween.easingMethod == EasingMethod.Curve)
            {
                tween.curve = EditorGUILayout.CurveField("Curve", tween.curve);
            }

            if (tween.easingMethod == EasingMethod.DoubleCurve)
            {
                tween.beginStateCurve = EditorGUILayout.CurveField("Begin State Curve", tween.beginStateCurve);
                tween.endStateCurve = EditorGUILayout.CurveField("End State Curve", tween.endStateCurve);
            }

            if (tween.easingMethod == EasingMethod.EaseIn ||
                tween.easingMethod == EasingMethod.EaseOut ||
                tween.easingMethod == EasingMethod.EaseInOut)
            {
                tween.easingType = (EasingType) EditorGUILayout.EnumPopup("Easing Type", tween.easingType);
            }

            tween.looping = (LoopType) EditorGUILayout.EnumPopup("Loop Type", tween.looping);

            DrawEvent("OnEndStateSet");
            DrawEvent("OnBeginStateSet");
            DrawEvent("OnEndStateSetStarted");
            DrawEvent("OnBeginStateSetStarted");

            DrawDebugButtons(tween);

            if (!subscribed && testInEditor)
            {
                subscribed = true;
                tween.SubscribeToEditorUpdates();
            }
            else if (subscribed && !testInEditor)
            {
                subscribed = false;
                tween.UnsubscribeFromEditorUpdates();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void OnDisable()
        {
            if (subscribed && tween != null)
            {
                subscribed = false;
                tween.UnsubscribeFromEditorUpdates();
            }

            testInEditor = false;
        }

        private void OnDestroy()
        {
            if (tween != null)
            {
                tween.UnsubscribeFromEditorUpdates();
            }
        }

        private void OnEnable()
        {
            testInEditor = true;
        }

        void DrawEvent(string eventName)
        {
            SerializedProperty onCheck = serializedObject.FindProperty(eventName);

            EditorGUILayout.PropertyField(onCheck);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        static void DrawDebugButtons(MBTweenBase tween)
        {
            Color defColor = GUI.color;

            GUI.color = tween.IsInBeginState ? Color.green : defColor;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Set End State"))
            {
                tween.SetEndState();
            }

            EditorGUILayout.EndHorizontal();


            GUI.color = tween.IsInEndState ? Color.green : defColor;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Set Begin State"))
            {
                tween.SetBeginState();
            }

            EditorGUILayout.EndHorizontal();

            GUI.color = defColor;
        }
    }
}