using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UIFramework.Runtime;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

public class RewardsUIFeedbackService : ITickable, IPostStartable
{
    private readonly UIFrame _uiFrame;
    Dictionary<UIFeedbackTarget, IRewardUIPositionProvider> _positionProviders = new();

    List<AnimatedUIReward> _animatedRewards = new();

    private RewardsUIFeedbackScreen _rewardsUIFeedbackScreen;

    public RewardsUIFeedbackService(UIFrame uiFrame)
    {
        _uiFrame = uiFrame;
    }

    public void PostStart()
    {
        _rewardsUIFeedbackScreen = _uiFrame.Open<RewardsUIFeedbackScreen>();
    }

    public void RegisterPositionProvider(UIFeedbackTarget target, IRewardUIPositionProvider provider)
    {
        if (_positionProviders.ContainsKey(target) && _positionProviders[target] != null)
        {
            Debug.LogError($"Position provider for {target} already registered");
        }

        _positionProviders[target] = provider;
    }

    public void UnregisterPositionProvider(UIFeedbackTarget target)
    {
        _positionProviders.Remove(target);
    }

    public async void PlayRewardFeedback(RewardFeedbackData data)
    {
        if (!_positionProviders.TryGetValue(data.Target, out var positionProvider))
        {
            Debug.LogError($"Position provider for {data.Target} not found");
            return;
        }

        int rewardsCount = data.RewardsCount > 0 ? data.RewardsCount : 1;
        int animationPointsCount = data.AnimationPointsCount > 0 ? data.AnimationPointsCount : 3;
        var targetPosition = positionProvider.GetRewardTargetPosition();

        for (int i = 0; i < rewardsCount; i++)
        {
            data.StartPosition.z = targetPosition.z;
            var animationPoints = GenerateAnimationPoints(data.StartPosition, targetPosition, animationPointsCount);

            var reward = new AnimatedUIReward
            {
                Target = data.Target,
                AnimationTime = data.AnimationTime,
                AnimationPoints = animationPoints,
                Progress = 0,
                OnComplete = i == rewardsCount - 1 ? data.OnComplete : null,
                RectTransform = _rewardsUIFeedbackScreen.GetRewardPrefab(data.Target, data.StartPosition)
                    .GetComponent<RectTransform>()
            };

            _animatedRewards.Add(reward);

            await UniTask.Delay(TimeSpan.FromSeconds(data.SpawnDelay));
        }
    }

    private static Vector3[] GenerateAnimationPoints(Vector3 startPosition, Vector3 targetPosition, int pointsCount)
    {
        if (pointsCount == 2)
        {
            return new[] {startPosition, targetPosition};
        }

        var diff = targetPosition - startPosition;

        Vector3 normal = new Vector3(-diff.y, diff.x, 0);

        if (Random.Range(-1f, 1f) > 0)
        {
            normal = -normal;
        }

        if (pointsCount == 3)
        {
            return new[]
            {
                startPosition, startPosition + diff * Random.Range(0.25f, 0.75f) + normal * Random.Range(0, 0.75f),
                targetPosition
            };
        }

        if (pointsCount == 4)
        {
            float normalMultiplier = Mathf.Sign(Random.Range(-1f, 1f));
            return new[]
            {
                startPosition,
                startPosition + diff * Random.Range(-0.5f, 0.2f) + normal * Random.Range(0, 0.5f),
                startPosition + diff * Random.Range(0.6f, 1f) + normal * (normalMultiplier * Random.Range(0, 0.5f)),
                targetPosition
            };
        }


        return new[] {startPosition, targetPosition};
    }

    public void Tick()
    {
        for (int i = _animatedRewards.Count - 1; i >= 0; i--)
        {
            AnimatedUIReward animatedReward = _animatedRewards[i];

            animatedReward.Progress += Time.deltaTime / animatedReward.AnimationTime;

            var providerExists = _positionProviders.TryGetValue(animatedReward.Target, out var positionProvider);
            
            if (animatedReward.Progress >= 1 || !providerExists ||
                animatedReward.CancellationToken.IsCancellationRequested)
            {
                _rewardsUIFeedbackScreen.ReturnRewardPrefab(animatedReward.Target,
                    animatedReward.RectTransform.gameObject);
                _animatedRewards.RemoveAt(i);

                if (!animatedReward.CancellationToken.IsCancellationRequested)
                    animatedReward.OnComplete?.Invoke();
            }
            else 
            {
                animatedReward.AnimationPoints[^1] = positionProvider.GetRewardTargetPosition();
                animatedReward.RectTransform.position =
                    Bezier.GetPoint(animatedReward.AnimationPoints, animatedReward.Progress);
            }
        }
    }
}