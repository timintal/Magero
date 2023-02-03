using UnityEngine;
using System;
using System.Collections.Generic;

using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#if !UNITY_2021_1_OR_NEWER
	using UnityEditor.Experimental.SceneManagement;
#endif
#endif
namespace MBTweens
{
    public class MBTweenAnimation : MBTweenAnimationAbstract
    {
        [Serializable]
        public struct AnimationEntry
        {
            public MBTweenAnimation animation;
            public float delay;
        }

        [Serializable]
        public struct TweenEntry
        {
            public MBTweenBase tween;
            public float delay;
            public bool isForwardOnlyTween;
        }

        #region Fields

        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool needSortTweens;
        [SerializeField] GameObject rootToDeactivate;
        [SerializeField] private Renderer[] renderersToDeactivate = new Renderer[0];
        [SerializeField] AnimationEntry[] animations = new AnimationEntry[0];
        [SerializeField] private List<TweenEntry> tweens = new List<TweenEntry>();
        [SerializeField] LoopType looping;
        [SerializeField] UnityEvent OnEndStateSet;
        [SerializeField] UnityEvent OnBeginStateSet;

        [SerializeField] GameObject[] additionalsToDeactivate = new GameObject[0];

        float totalDuration = -100;

        [SerializeField][HideInInspector] float durationScale = 1;


        #endregion

        #region Properties

        public override UnityEvent OnEndStateSetEvent
        {
            get { return OnEndStateSet; }
        }

        public override UnityEvent OnBeginStateSetEvent
        {
            get { return OnBeginStateSet; }
        }

        public AnimationEntry[] Animations => animations;


        public List<TweenEntry> Tweens
        {
            get { return tweens; }
            set { tweens = value; }
        }

        public float TotalDuration
        {
            get
            {
#if UNITY_EDITOR
                totalDuration = -1;
#endif

                if (totalDuration < 0)
                {
                    for (int i = 0; i < tweens.Count; i++)
                    {
                        TweenEntry t = tweens[i];
                        if (t.tween.duration * t.tween.durationScale + t.delay * DurationScale > totalDuration)
                        {
                            totalDuration = t.tween.duration * t.tween.durationScale + t.delay  * DurationScale;
                        }
                    }

                    for (int i = 0; i < animations.Length; i++)
                    {
                        AnimationEntry anim = animations[i];
                        if (anim.animation.TotalDuration + anim.delay > totalDuration)
                        {
                            totalDuration = anim.animation.TotalDuration + anim.delay * DurationScale;
                        }
                    }
                }

                return totalDuration;
            }
        }
        
        public float TotalReverseDuration
        {
            get
            {
#if UNITY_EDITOR
                totalDuration = -1;
#endif

                if (totalDuration < 0)
                {
                    for (int i = 0; i < tweens.Count; i++)
                    {
                        TweenEntry t = tweens[i];
                        if (!t.isForwardOnlyTween && t.tween.duration * t.tween.durationScale + t.delay * DurationScale > totalDuration)
                        {
                            totalDuration = t.tween.duration * t.tween.durationScale + t.delay * DurationScale;
                        }
                    }

                    for (int i = 0; i < animations.Length; i++)
                    {
                        AnimationEntry anim = animations[i];
                        if (anim.animation.TotalDuration + anim.delay * DurationScale > totalDuration)
                        {
                            totalDuration = anim.animation.TotalDuration + anim.delay * DurationScale;
                        }
                    }
                }

                return totalDuration;
            }
        }

        public float DurationScale
        {
            get { return durationScale; }
            set
            {
                durationScale = value;
                for (int i = 0; i < tweens.Count; i++)
                {
                    TweenEntry t = tweens[i];
                    if (t.tween != null)
                    {
                        t.tween.durationScale = durationScale;
                    }
                }

                for (int i = 0; i < animations.Length; i++)
                {
                    AnimationEntry anim = animations[i];
                    anim.animation.DurationScale = durationScale;
                }
            }
        }

        public override bool IsAnimationRunning { get; set; }

        public override bool IsInBeginState
        {
            get
            {

                for (int i = 0; i < tweens.Count; i++)
                {
                    TweenEntry t = tweens[i];
                    if (!t.tween.IsInBeginState)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < animations.Length; i++)
                {
                    AnimationEntry anim = animations[i];
                    if (!anim.animation.IsInBeginState)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public override bool IsInEndState
        {
            get
            {
                if (rootToDeactivate != null && !rootToDeactivate.gameObject.activeSelf)
                    return false;
                for (int i = 0; i < tweens.Count; i++)
                {
                    TweenEntry t = tweens[i];

                    if (!t.tween.IsInEndState)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < animations.Length; i++)
                {
                    AnimationEntry anim = animations[i];

                    if (!anim.animation.IsInEndState)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool IsReady { get; private set; }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            if (playOnAwake)
            {
                SetEndState();
            }
            else
            {
                enabled = false;
            }

            if (OnEndStateSet == null)
            {
                OnEndStateSet = new UnityEvent();
            }
            
            if (OnBeginStateSet == null)
            {
                OnBeginStateSet = new UnityEvent();
            }

            IsReady = true;
        }

#if UNITY_EDITOR
        void EditorUpdate()
        {
            if (this == null)
            {
                UnsubscribeFromEditorUpdates();
                return;
            }
            
            if (!EditorApplication.isPlaying && enabled)
            {
                Update();
            }
        }
#endif

        void Update()
        {
            if (isPaused)
            {
                return;
            }
            
            if (IsAnimationRunning)
            {
                bool allTweensFinished = true;

                for (int i = 0; i < tweens.Count; i++)
                {
                    var t = tweens[i];
                    if (t.tween.enabled)
                    {
                        allTweensFinished = false;
                        break;
                    }
                }

                for (int i = 0; i < animations.Length; i++)
                {
                    var an = animations[i];
                    if (an.animation.IsAnimationRunning)
                    {
                        allTweensFinished = false;
                    }
                }

                if (allTweensFinished)
                {
                    IsAnimationRunning = false;
                    #if UNITY_EDITOR
                    var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (prefabStage != null) {
                        EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                    }
                    #endif

                    if (IsInBeginState)
                    {
                        OnBeginStateSet.Invoke();

                        if (looping == LoopType.Loop)
                        {
                            SetBeginState();
                        }
                        else if (looping == LoopType.PingPong)
                        {
                            SetEndState();
                        }
                        else
                        {
                            if (rootToDeactivate != null)
                            {
                                rootToDeactivate.SetActive(false);
                            }

                            for (int i = 0; i < renderersToDeactivate.Length; i++)
                            {
                                renderersToDeactivate[i].enabled = false;
                            }

                            if (additionalsToDeactivate != null && additionalsToDeactivate.Length != 0)
                            {
                                for (int i = 0; i < additionalsToDeactivate.Length; i++)
                                {
                                    additionalsToDeactivate[i].SetActive(false);
                                }
                            }
                        }
                    }
                    else if (IsInEndState)
                    {
                        OnEndStateSet.Invoke();

                        if (looping == LoopType.Loop)
                        {
                            SetEndState();
                        }
                        else if (looping == LoopType.PingPong)
                        {
                            SetBeginState();
                        }
                    }
                   
                }
            }
            else
            {
                enabled = false;
            }
        }

        #endregion

        #region Public Methods

        private bool isPaused;
        public void Pause()
        {
            foreach (var tweenEntry in tweens)
            {
                tweenEntry.tween.enabled = false;
            }

            isPaused = true;
        }
        
        public void Unpause()
        {
            foreach (var tweenEntry in tweens)
            {
                tweenEntry.tween.enabled = true;
            }

            isPaused = false;
        }

        public void SubscribeToEditorUpdates()
        {
#if UNITY_EDITOR
            EditorApplication.update += EditorUpdate;
            foreach (var tween in tweens)
            {
                tween.tween.SubscribeToEditorUpdates();
            }

            foreach (var a in animations)
            {
                a.animation.SubscribeToEditorUpdates();
            }
#endif
        }

        public void UnsubscribeFromEditorUpdates()
        {
#if UNITY_EDITOR
            EditorApplication.update -= EditorUpdate;
            foreach (var tween in tweens)
            {
                tween.tween.UnsubscribeFromEditorUpdates();
            }

            foreach (var a in animations)
            {
                a.animation.UnsubscribeFromEditorUpdates();
            }
#endif
        }

        public override void SetEndState(float delay = 0)
        {
            if (rootToDeactivate != null)
            {
                rootToDeactivate.SetActive(true);
            }

            for (int i = 0; i < renderersToDeactivate.Length; i++)
            {
                renderersToDeactivate[i].enabled = true;
            }

            if (additionalsToDeactivate != null && additionalsToDeactivate.Length != 0)
            {
                for (int i = 0; i < additionalsToDeactivate.Length; i++)
                {
                    additionalsToDeactivate[i].SetActive(true);
                }
            }

            if (needSortTweens)
            {
                tweens.Sort((entry, otherEntry) => { return otherEntry.delay.CompareTo(entry.delay); });
            }

            for (int i = 0; i < tweens.Count; i++)
            {
                var t = tweens[i];
                if (!t.tween.smooth)
                {
                    t.tween.SetBeginStateImmediately();
                }
                t.tween.SetEndState((t.delay + delay) * DurationScale, t.tween.duration);
            }

            for (int i = 0; i < animations.Length; i++)
            {
                var a = animations[i];
                a.animation.SetBeginStateImmediately();
                a.animation.SetEndState((a.delay + delay) * DurationScale);
            }

            IsAnimationRunning = true;
            enabled = true;
        }

        public override void SetBeginState(float delay = 0)
        {
            if (rootToDeactivate != null)
            {
                rootToDeactivate.SetActive(true);
            }

            
            if (needSortTweens)
            {
                tweens.Sort((entry, otherEntry) => { return entry.delay.CompareTo(otherEntry.delay); });
            }
            
            for (int i = 0; i < tweens.Count; i++)
            {
                var t = tweens[i];
                if (t.isForwardOnlyTween)
                {
                    t.tween.SetBeginStateImmediately();
                }
                else
                {
                    if (!t.tween.smooth)
                    {
                        t.tween.SetEndStateImmediately();
                    }
                    t.tween.SetBeginState(TotalReverseDuration - ((t.delay * DurationScale + t.tween.duration * t.tween.durationScale) + delay),
                        t.tween.duration);
                }
            }

            for (int i = 0; i < animations.Length; i++)
            {
                var a = animations[i];
                a.animation.SetEndStateImmediately();
                a.animation.SetBeginState(TotalDuration - (a.delay * DurationScale + a.animation.TotalDuration) + delay * DurationScale);
            }

            IsAnimationRunning = true;
            enabled = true;
        }

        public override void SetBeginStateImmediately()
        {
            if (needSortTweens)
            {
                tweens.Sort((entry, otherEntry) => { return otherEntry.delay.CompareTo(entry.delay); });
            }
            
            for (int i = 0; i < tweens.Count; i++)
            {
                var t = tweens[i];
                t.tween.SetBeginStateImmediately();
            }

            for (int i = 0; i < animations.Length; i++)
            {
                var a = animations[i];
                a.animation.SetBeginStateImmediately();
            }

            if (rootToDeactivate != null)
            {
                rootToDeactivate.SetActive(false);
            }

            for (int i = 0; i < renderersToDeactivate.Length; i++)
            {
                renderersToDeactivate[i].enabled = false;
            }

            if (additionalsToDeactivate != null && additionalsToDeactivate.Length != 0)
            {
                for (int i = 0; i < additionalsToDeactivate.Length; i++)
                {
                    additionalsToDeactivate[i].SetActive(false);
                }
            }

            enabled = false;
            IsAnimationRunning = false;
        }

        public override void SetEndStateImmediately()
        {
            if (rootToDeactivate != null)
            {
                rootToDeactivate.SetActive(true);
            }

            if (needSortTweens)
            {
                tweens.Sort((entry, otherEntry) => { return otherEntry.delay.CompareTo(entry.delay); });
            }
            
            for (int i = 0; i < renderersToDeactivate.Length; i++)
            {
                renderersToDeactivate[i].enabled = true;
            }

            if (additionalsToDeactivate != null && additionalsToDeactivate.Length != 0)
            {
                for (int i = 0; i < additionalsToDeactivate.Length; i++)
                {
                    additionalsToDeactivate[i].SetActive(true);
                }
            }

            for (int i = 0; i < tweens.Count; i++)
            {
                var t = tweens[i];
                t.tween.SetEndStateImmediately();
            }

            for (int i = 0; i < animations.Length; i++)
            {
                var a = animations[i];
                a.animation.SetEndStateImmediately();
            }

            enabled = false;
            IsAnimationRunning = false;
        }

        private void OnDestroy()
        {
            UnsubscribeFromEditorUpdates();
        }

        #endregion

        #region Private Methods

        #endregion

        #region Event Handlers

        #endregion

    }
}