using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace MBTweens
{
public enum EasingMethod
{
    None,
    Curve,
    EaseIn,
    EaseOut,
    EaseInOut,
    DoubleCurve
}

public enum LoopType
{
    None,
    Loop,
    PingPong
}

public class MBTweenBase : MonoBehaviour 
{
    #region Events

    public Action OnCompleteAction;

    #endregion


    #region Fields

    [HideInInspector] public bool playOnAwake;
    [HideInInspector] public float duration;
    [HideInInspector] public float durationScale = 1;
    [HideInInspector] public bool smooth = false;
    
    [HideInInspector] public float delay;

    [HideInInspector] public EasingMethod easingMethod;
    [HideInInspector] public EasingType easingType;
    [HideInInspector] public AnimationCurve curve;
    [HideInInspector] public LoopType looping;
    
    [HideInInspector] public AnimationCurve beginStateCurve;
    [HideInInspector] public AnimationCurve endStateCurve;

    [HideInInspector] public bool ignoreTimeScale;
   
    [HideInInspector] public UnityEvent OnEndStateSet;
    [HideInInspector] public UnityEvent OnBeginStateSet;

    [HideInInspector] public UnityEvent OnEndStateSetStarted;
    [HideInInspector] public UnityEvent OnBeginStateSetStarted;

    float tweenFactor;
    float currentTime;
    float currentDelay;
    float timeStepMultiplier;
    private float selectedDelay = 0f;

    private AnimationCurve currentCurve;

#if UNITY_EDITOR
    private double previousEditorTime;
    private float editorDeltaTime;
#endif

    #endregion

    #region Properties

    public virtual bool IsInBeginState
    {
        get
        {
            if (enabled || timeStepMultiplier > 0)
            {
                return false;
            }

            if (currentTime / (duration * durationScale) > float.Epsilon)
            {
                return false;
            }

            return true;
        }
    }

    public virtual bool IsInEndState
    {
        get
        {
            if (enabled || timeStepMultiplier < 0)
            {
                return false;
            }

            if (currentTime / (duration * durationScale) < 1.0f)
            {
                return false;
            }

            return true;
        }
    }

    #endregion


    #region Unity Lifecycle



    protected virtual void Awake()
    {
        if (playOnAwake)
        {
            SetBeginStateImmediately();
            SetEndState();
        }
        else
        {
            enabled = false;
        }
    }

#if UNITY_EDITOR
    void EditorUpdate()
    {
        editorDeltaTime = (float)(EditorApplication.timeSinceStartup - previousEditorTime);
        previousEditorTime = EditorApplication.timeSinceStartup;

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

    void Update()
    {
        float currentDeltaTime = GetDeltaTime();

        //check Delay
        currentDelay += currentDeltaTime;

        if (currentDelay < selectedDelay)
        {
            return;
        }
        
        currentTime += currentDeltaTime * timeStepMultiplier;

        if (timeStepMultiplier > 0 && currentTime > duration * durationScale)
        {
            tweenFactor = GetTweenFactor(1);
            enabled = false;

            currentTime = duration * durationScale;
            
            if (OnCompleteAction != null)
            {
                OnCompleteAction();
            }

            EndStateSet();
        }
        else if (timeStepMultiplier < 0 && currentTime < 0)
        {
            tweenFactor = GetTweenFactor(0);
            enabled = false;
            currentTime = 0;

            if (OnCompleteAction != null)
            {
                OnCompleteAction();
            }

            BeginStateSet();
        }
        else
        {
            tweenFactor = GetTweenFactor(currentTime / (duration * durationScale));
        }

        UpdateTweenWithFactor(tweenFactor);  
    }


    #endregion


    #region Public Methods

    public void Stop()
    {
        enabled = false;
    }

    public void Resume()
    {
        enabled = false;
    }
    
    protected float GetStartFactor(int startFactor)
    {
        if (enabled)
        {
            return currentTime / duration;
        }

        float factor = GetFactor(startFactor);
        return GetTweenFactor(factor);
    }

    protected virtual float GetFactor(int startFactor)
    {
        return startFactor;
    }
    
    public void SetBeginState()
    {
        SetBeginState(delay, duration);
    }

    public virtual void SetBeginState(float newDelay, float newDuration)
    {
        selectedDelay = newDelay;
        this.duration = newDuration;

        var factor = GetStartFactor(1);
        tweenFactor = factor;
        enabled = true;
        timeStepMultiplier = -1;
        currentTime = this.duration * durationScale * factor;
        currentDelay = 0;

        OnBeginStateSetStarted?.Invoke();

        SetCurrentCurve(true);
    }

    public void SetEndState()
    {
        SetEndState(delay, duration);
    }

    public virtual void SetEndState(float newDelay, float newDuration)
    {
        selectedDelay = newDelay;
        duration = newDuration;

        var factor = GetStartFactor(0);
        tweenFactor = factor;
        enabled = true;
        timeStepMultiplier = 1;
        currentTime = this.duration * durationScale * factor;
        currentDelay = 0;

        OnEndStateSetStarted?.Invoke();

        SetCurrentCurve(false);
    }

    public void SetBeginStateImmediately()
    {
        SetCurrentCurve(true);
        tweenFactor = 0;
        timeStepMultiplier = -1;
        currentTime = 0;
        UpdateTweenWithFactor(GetTweenFactor(tweenFactor));
        enabled = false;
        OnBeginStateSetStarted?.Invoke();
    }

    public void SetEndStateImmediately()
    {
        SetCurrentCurve(false);
        currentTime = duration * durationScale;
        timeStepMultiplier = 1;
        tweenFactor = 1;
        UpdateTweenWithFactor(GetTweenFactor(tweenFactor));
        enabled = false;
        OnEndStateSetStarted?.Invoke();
    }

    private void SetCurrentCurve(bool isBeginState)
    {
        if (easingMethod == EasingMethod.Curve)
        {
            currentCurve = curve;
        }
        else if (easingMethod == EasingMethod.DoubleCurve)
        {
            currentCurve = isBeginState ? beginStateCurve : endStateCurve;
        }
    }

   public void SubscribeToEditorUpdates()
    {
#if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
#endif
    }

    public void UnsubscribeFromEditorUpdates()
    {
#if UNITY_EDITOR
        EditorApplication.update -= EditorUpdate;
#endif
    }

    #endregion

    #region Private Methods

    protected virtual void UpdateTweenWithFactor(float factor)
    {
        
    }

    void Reset()
    {
        curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        beginStateCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        endStateCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        durationScale = 1;
    }

    float GetTweenFactor(float relativeTime)
    {
        if (easingMethod == EasingMethod.None)
        {
            return relativeTime;
        }
        else if (easingMethod == EasingMethod.EaseIn)
        {
            return Easing.EaseIn(relativeTime, easingType);
        }
        else if (easingMethod == EasingMethod.EaseOut)
        {
            return Easing.EaseOut(relativeTime, easingType);
        }
        else if (easingMethod == EasingMethod.EaseInOut)
        {
            return Easing.EaseInOut(relativeTime, easingType);
        }
        else if (currentCurve != null && (easingMethod == EasingMethod.Curve || easingMethod == EasingMethod.DoubleCurve))
        {
            return currentCurve.Evaluate(relativeTime);
        }

        return relativeTime;
    }

    void BeginStateSet()
    {
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
            OnBeginStateSet?.Invoke();
        }
    }

    void EndStateSet()
    {
        if (looping == LoopType.Loop)
        {
            SetEndState();
        }
        else if (looping == LoopType.PingPong)
        {
            SetBeginState();
        }
        else
        {
            OnEndStateSet?.Invoke();
        }
    }

    #endregion
}
}