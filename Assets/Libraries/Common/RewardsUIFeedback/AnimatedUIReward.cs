using System;
using System.Threading;
using UnityEngine;

public class AnimatedUIReward
{
    public UIFeedbackTarget Target;
    public float AnimationTime;
        
    public Vector3[] AnimationPoints;

    public float Progress;
        
    public Action OnComplete;
        
    public RectTransform RectTransform;
    
    public CancellationToken CancellationToken;
}