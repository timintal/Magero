using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EasyTweens
{
    [CustomEditor(typeof(TweenAnimation))]
    public class TweenAnimationEditor : Editor
    {
        private VisualElement rootElement;
        private TweenAnimation animation;

        public TweenAnimation Animation => animation;

        private List<Type> tweenTypes = new List<Type>();
        private List<TweenEditor> tweenEditors = new List<TweenEditor>();

        private Action<Vector2> factorUpdateAction;
        
        public void OnEnable()
        {
            animation = (TweenAnimation) target;
            animation.SubscribeToEditorUpdates();
            rootElement = new VisualElement();
            
            // Load in UXML template and USS styles, then apply them to the root element.
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("TweenAnimation");
            visualTree.CloneTree(rootElement);

            StyleSheet stylesheet = Resources.Load<StyleSheet>("TweenAnimation");
            rootElement.styleSheets.Add(stylesheet);
        }

        public void OnDisable()
        {
            animation.UnsubscribeFromEditorUpdates();
        }

        public override VisualElement CreateInspectorGUI()
        {
            Button playForward = rootElement.Q<Button>("PlayForward");
            playForward.clickable.clicked += () =>
            {
                animation.PlayForward();
            };
            Button playFastForward = rootElement.Q<Button>("PlayFastForward");
            playFastForward.clickable.clicked += () =>
            {
                animation.PlayForward(false);
            };
            
            Button playBackward = rootElement.Q<Button>("PlayBackward");
            playBackward.clickable.clicked += () =>
            {
                animation.PlayBackward();
            };
            Button playFastBackward = rootElement.Q<Button>("PlayFastBackward");
            playFastBackward.clickable.clicked += () =>
            {
                animation.PlayBackward(false);
            };

            Button minimizeDuration = rootElement.Q<Button>("MinimizeDuration");
            minimizeDuration.clickable.clicked += () =>
            {
                animation.duration = 0;
                animation.CheckDuration();
            };
                
            SerializedObject animObj = new SerializedObject(animation);

            SerializedProperty playOnAwakeProperty = animObj.FindProperty("PlayOnAwake");
            Toggle playOnAwakeToggle = rootElement.Q<Toggle>("PlayOnAwake");
            playOnAwakeToggle.BindProperty(playOnAwakeProperty);
            playOnAwakeToggle.RegisterCallback<ChangeEvent<bool>>(evt =>
            {
                animation.PlayOnAwake = evt.newValue;
            });

            SerializedProperty animDurationProperty = animObj.FindProperty("duration");
            FloatField durationField = rootElement.Q<FloatField>("Duration");
            durationField.BindProperty(animDurationProperty);
            durationField.RegisterCallback<ChangeEvent<float>>(evt =>
            {
                animation.duration = evt.newValue;
                animation.CheckDuration();
                durationField.value = animation.duration;
                for (int i = 0; i < tweenEditors.Count; i++)
                {
                    tweenEditors[i].UpdateDelayDurationSlider();
                }
            }, TrickleDown.TrickleDown);

            SetupFactorProgressBar(animObj);

            EnumField loopEnum = rootElement.Q<EnumField>("LoopType");
            loopEnum.Init(animation.lootType);
            loopEnum.RegisterCallback<ChangeEvent<Enum>>(evt =>
            {
                animation.lootType = (LoopType)evt.newValue;
            });
            
            DrawTweens();

            return rootElement;
        }

        private void SetupFactorProgressBar(SerializedObject animObj)
        {
            ProgressBar factorProgressBar = rootElement.Q<ProgressBar>("Factor");
            Label label = rootElement.Q<Label>("FactorLabel");
            factorUpdateAction = pos =>
            {
                animation.enabled = false;
                float factor = pos.x / factorProgressBar.layout.width * animation.duration;
                // label.text = (factor * animation.duration).ToString("F1");
                animation.SetFactor(factor);
                factorProgressBar.value = pos.x / factorProgressBar.layout.width;
                factorProgressBar.title = (factor).ToString("F2");
            };
            //
            factorProgressBar.RegisterCallback<MouseDownEvent>(mde =>
            {
                factorUpdateAction(mde.localMousePosition);
            
                factorProgressBar.RegisterCallback<MouseMoveEvent>(MouseMoveOnFactorCallback());
            });
            //
            // factorProgressBar.RegisterCallback<MouseUpEvent>(mde =>
            // {
            //     factorUpdateAction(mde.localMousePosition);
            //
            //     factorProgressBar.UnregisterCallback(MouseMoveOnFactorCallback());
            // });
            //
            // factorProgressBar.RegisterCallback<MouseOutEvent>(mde =>
            // {
            //     factorUpdateAction(mde.localMousePosition);
            //
            //     factorProgressBar.UnregisterCallback(MouseMoveOnFactorCallback());
            // });

            FloatField timeField = rootElement.Q<FloatField>("TimeField");
            timeField.BindProperty(animObj.FindProperty("currentTime"));
            timeField.RegisterValueChangedCallback(evt => { factorProgressBar.value = evt.newValue / animation.duration; });
        }

        private EventCallback<MouseMoveEvent> MouseMoveOnFactorCallback()
        {
            return mde =>
            {
                factorUpdateAction(mde.localMousePosition);
            };
        }

        private void DrawTweens()
        {
            FillTweenEditors();

            Button clearButton = rootElement.Q<Button>("ClearTweens");
            clearButton.clickable.clicked += () => 
            {
                RemoveAllTweens();
            };
            
            FillTweenClassesForAddButton();
        }

        private void FillTweenEditors()
        {
            VisualElement tweensContainer = rootElement.Q<VisualElement>("TweensContainer");

            tweensContainer.Clear();
            tweenEditors.Clear();
            
            for (int i = 0; i < animation.tweens.Count; i++)
            {
                animation.tweens[i].gameObject.hideFlags = HideFlags.HideInHierarchy;
                TweenEditor te = new TweenEditor(this, animation.tweens[i]);
                tweensContainer.Add(te);
                tweenEditors.Add(te);
            }
        }


        void FillTweenClassesForAddButton()
        {
            IEnumerable<Type> exporters = typeof(TweenBase)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(TweenBase)) && !t.IsAbstract);
            
            var choices = new List<string>();
            foreach (var type in exporters)
            {
                tweenTypes.Add(type);
                PropertyInfo pi = type.GetProperty("TweenName",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);

                if (pi != null)
                {
                    choices.Add((string) pi.GetValue(null));
                }
                else
                {
                    choices.Add(type.Name);
                }
            }

            var tweensDropdown = rootElement.Q<DropdownField>("AddTween");
            PropertyInfo fi = typeof(DropdownField).GetProperty("choices", BindingFlags.NonPublic |BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
            fi.SetValue(tweensDropdown, choices);

            tweensDropdown.value = tweensDropdown.choices[0];

            Button addTween = rootElement.Q<Button>("AddTween");

            addTween.clickable.clicked += () => 
            {
                AddTween(tweenTypes[tweensDropdown.index]);
            };
        }

        void AddTween(Type tweenType)
        {
            GameObject newTweenGO = new GameObject(tweenType.Name);
            newTweenGO.transform.parent = animation.transform;
            newTweenGO.hideFlags = HideFlags.HideInHierarchy;
            TweenBase tween = (TweenBase)newTweenGO.AddComponent(tweenType);
            tween.curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            animation.tweens.Add(tween);
            FillTweenEditors();
        }

        public void RemoveTween(TweenEditor te)
        {
            int index = tweenEditors.IndexOf(te);
            animation.tweens.RemoveAt(index);
            
            FillTweenEditors();
        }
        
        public void DuplicateTween(TweenEditor te)
        {
            GameObject newTweenGo = Instantiate(te.Tween.gameObject);
            newTweenGo.transform.parent = animation.transform;
            newTweenGo.hideFlags = HideFlags.HideInHierarchy;
            TweenBase tween = newTweenGo.GetComponent<TweenBase>();
            animation.tweens.Add(tween);
            FillTweenEditors();
        }
        
        void RemoveAllTweens()
        {
            animation.tweens.Clear();
            VisualElement tweensContainer = rootElement.Q<VisualElement>("TweensContainer");
            tweensContainer.Clear();
            tweenEditors.Clear();

            for (int i = animation.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(animation.transform.GetChild(i).gameObject);
            }
        }
    }
}