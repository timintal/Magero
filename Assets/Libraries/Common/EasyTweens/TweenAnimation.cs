using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTweens
{
    public enum LoopType
    {
        None,
        Loop,
        PingPong
    }
    
    public class TweenAnimation : MonoBehaviour
    {
        public List<TweenBase> tweens = new List<TweenBase>();

        public float duration;
        public bool PlayOnAwake;

        public LoopType lootType = LoopType.None;
        public bool ignoreTimeScale; 

        public float currentTime;
        private int directionMultiplier = 1;

        public Action OnPlayForwardFinished;
        public Action OnPlayBackwardFinished;
        
        public bool IsInStartState => currentTime < 0.01f && !enabled;
        public bool IsInEndState => currentTime > duration * 0.99f && !enabled;
        
        #if UNITY_EDITOR
        private float editorDeltaTime;
        private double previousEditorTime;
        #endif

        private void Awake()
        {
            if (PlayOnAwake)
            {
                SetFactor(0);
                PlayForward();
            }
        }

        public void PlayForward(bool animated = true)
        {
            if (!animated)
            {
                SetFactor(1);
            }
            else
            {
                currentTime = 0;
                directionMultiplier = 1;
                Play();
            }
        }

        public void PlayBackward(bool animated = true)
        {
            if (!animated)
            {
                SetFactor(0);
            }
            else
            {
                currentTime = duration;
                directionMultiplier = -1;
                Play();
            }
        }

        void Play()
        {
            enabled = true;
            CheckDuration();
        }

        public void CheckDuration()
        {
            for (int i = 0; i < tweens.Count; i++)
            {
                if (tweens[i].duration + tweens[i].delay > duration)
                {
                    duration = tweens[i].duration + tweens[i].delay;
                }
            }
        }
        
        private void Update()
        {
            if (enabled)
            {
                currentTime += GetDeltaTime() * directionMultiplier;

                if (currentTime < 0 && directionMultiplier < 0)
                {
                    OnPlayBackwardFinished?.Invoke();
                    FinishAnimationCycle();
                }
                else if (currentTime > duration && directionMultiplier > 0)
                {
                    OnPlayForwardFinished?.Invoke();
                    FinishAnimationCycle();
                }

                SetFactor(currentTime);
            }
        }

        void FinishAnimationCycle()
        {
            switch (lootType)
            {
                case LoopType.None:
                    enabled = false;
                    break;
                case LoopType.Loop:
                    currentTime = directionMultiplier > 0 ? 0 : duration;
                    break;
                case LoopType.PingPong:
                    directionMultiplier *= -1;
                    break;
                default:
                    break;
            }
        }

        public void SetFactor(float time)
        {
            for (int i = 0; i < tweens.Count; i++)
            {
                tweens[i].UpdateTween(time);
            }
        }
        
        
#if UNITY_EDITOR

        [ContextMenu("Show All Children")]
        void ShowAllChildren()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.gameObject.hideFlags = HideFlags.None;
            }
        }
        
        public void SubscribeToEditorUpdates()
        {
            EditorApplication.update += EditorUpdate;
        }

        public void UnsubscribeFromEditorUpdates()
        {
            EditorApplication.update -= EditorUpdate;
        }
        
        void EditorUpdate()
        {
            editorDeltaTime = (float)(EditorApplication.timeSinceStartup - previousEditorTime);
            previousEditorTime = EditorApplication.timeSinceStartup;

            try
            {
                if (!EditorApplication.isPlaying && enabled)
                {
                    Update();
                }
            }
            catch 
            {
                //could throw when prefab editor is closed, we don't care
            }
        }
#endif
        
        float GetDeltaTime()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return editorDeltaTime;
            }
#endif
            return ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
        }
    }
}

