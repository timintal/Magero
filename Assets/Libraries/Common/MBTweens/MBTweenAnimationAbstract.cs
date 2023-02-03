using UnityEngine;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class MBTweenAnimationAbstract : MonoBehaviour
{
    public abstract UnityEvent OnEndStateSetEvent { get; }
    public abstract UnityEvent OnBeginStateSetEvent { get; }
    
    public abstract bool IsAnimationRunning { get; set; }
    public abstract bool IsInBeginState { get; }
    public abstract bool IsInEndState { get; }


    public abstract void SetEndState(float delay = 0);
    public abstract void SetBeginState(float delay = 0);

    public abstract void SetBeginStateImmediately();
    public abstract void SetEndStateImmediately();

}