using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Object = UnityEngine.Object;

namespace EasyTweens
{
    public class TweenEditor : VisualElement
    {
        private TweenBase tween;
        private Foldout mainFoldout;
        private TweenAnimationEditor mainAnimationEditor;

        private MinMaxSlider delayDurationSlider;

        public TweenBase Tween => tween;

        public TweenEditor(TweenAnimationEditor animationEditor, TweenBase t)
        {
            tween = t;
            mainAnimationEditor = animationEditor;
            
            // Load in UXML template and USS styles, then apply them to the root element.
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("TweenEditor");
            visualTree.CloneTree(this);

            StyleSheet stylesheet = Resources.Load<StyleSheet>("TweenEditor");
            this.styleSheets.Add(stylesheet);

            mainFoldout = this.Q<Foldout>("MainFoldout");
            mainFoldout.value = false;
            SetFoldoutText(String.Empty);
            
            Button setCurrentToStartButton = this.Q<Button>("SetCurrentToStart");
            setCurrentToStartButton.clickable.clicked += () =>
            {
                tween.SetCurrentAsStartValue();
            };
            
            Button setCurrentToEndButton = this.Q<Button>("SetCurrentToEnd");
            setCurrentToEndButton.clickable.clicked += () =>
            {
                tween.SetCurrentAsEndValue();
            };
            
            Button removeTween = this.Q<Button>("RemoveTween");
            removeTween.clickable.clicked += () =>
            {
                mainAnimationEditor.RemoveTween(this);
            };
            
            Button duplicateTween = this.Q<Button>("DuplicateTween");
            duplicateTween.clickable.clicked += () =>
            {
                mainAnimationEditor.DuplicateTween(this);
            };
            
            BindProperties();
        }

        private void SetFoldoutText(string goName)
        {
            PropertyInfo pi = tween.GetType().GetProperty("TweenName",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);

            mainFoldout.text = ((string) pi.GetValue(null)) + goName;
        }

        void BindProperties()
        {
            PropertyField target = this.Q<PropertyField>("TargetField");
            PropertyField start = this.Q<PropertyField>("StartValue");
            PropertyField end = this.Q<PropertyField>("EndValue");
            CurveField curveField = this.Q<CurveField>("Curve");
            FloatField delayField = this.Q<FloatField>("Delay");
            FloatField durationField = this.Q<FloatField>("Duration");
            
            
            SerializedObject so = new SerializedObject(tween);
            SerializedProperty targetProp = so.FindProperty("target");
            SerializedProperty startProp = so.FindProperty("startValue");
            SerializedProperty endProp = so.FindProperty("endValue");
            SerializedProperty curveProp = so.FindProperty("curve");
            SerializedProperty delayProp = so.FindProperty("delay");
            SerializedProperty durationProp = so.FindProperty("duration");
            
            target.BindProperty(targetProp);
            start.BindProperty(startProp);
            end.BindProperty(endProp);
            curveField.BindProperty(curveProp);
            delayField.BindProperty(delayProp);
            durationField.BindProperty(durationProp);
            
            durationField.RegisterCallback<ChangeEvent<float>>(evt =>
            {
                tween.duration = evt.newValue;
                mainAnimationEditor.Animation.CheckDuration();
                UpdateDelayDurationSlider();
            });
            
            delayField.RegisterCallback<ChangeEvent<float>>(evt =>
            {
                tween.delay = evt.newValue;
                mainAnimationEditor.Animation.CheckDuration();
                UpdateDelayDurationSlider();
            });
            
            target.RegisterCallback<ChangeEvent<Object>>(ev =>
            {
                SetFoldoutText(ev != null && ev.newValue != null ? "->" + ev.newValue.name : "");
            });
            
            //MinMaxSlider
            delayDurationSlider = this.Q<MinMaxSlider>("DelayDurationSlider");
            UpdateDelayDurationSlider();
            delayDurationSlider.RegisterCallback<PointerUpEvent>(evt =>
            {
                mainAnimationEditor.Animation.CheckDuration();
                tween.delay = delayDurationSlider.minValue * mainAnimationEditor.Animation.duration;
                tween.duration = delayDurationSlider.maxValue * mainAnimationEditor.Animation.duration - tween.delay;
            }, TrickleDown.TrickleDown);

        }

        public void UpdateDelayDurationSlider()
        {
            delayDurationSlider.minValue = tween.delay / mainAnimationEditor.Animation.duration;
            delayDurationSlider.maxValue = (tween.delay + tween.duration) / mainAnimationEditor.Animation.duration;

        }

    }
}