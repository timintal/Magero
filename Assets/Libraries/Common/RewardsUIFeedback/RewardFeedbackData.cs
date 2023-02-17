using System;
using UnityEngine;

public struct RewardFeedbackData
{
    public UIFeedbackTarget Target;
    public int RewardsCount;
    public float AnimationTime;
    public float SpawnDelay;
    public Vector3 StartPosition;
    public Action OnComplete;
    public int AnimationPointsCount;
}