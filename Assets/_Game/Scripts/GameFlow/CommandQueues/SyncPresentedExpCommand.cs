using System;
using System.Threading;
using _Game.Data;
using _Game.Flow;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SyncPresentedExpCommand : Command
{
    private readonly RewardsUIFeedbackService _rewardsUIFeedbackService;
    private readonly PlayerData _playerData;
    private readonly Vector3 _uiStartPosition;

    public SyncPresentedExpCommand(RewardsUIFeedbackService rewardsUIFeedbackService, PlayerData playerData, Vector3 uiStartPosition)
    {
        _rewardsUIFeedbackService = rewardsUIFeedbackService;
        _playerData = playerData;
        _uiStartPosition = uiStartPosition;
    }
    
    
    public override async UniTask Execute(CancellationToken token)
    {
        if (_playerData.TotalExpPresented == _playerData.TotalExp)
            return;

        int expToAdd =  _playerData.TotalExp - _playerData.TotalExpPresented;
        
        int steps = Mathf.Min(expToAdd, 20);
        
        int stepExp = expToAdd / steps;
        
        for (int i = 0; i < steps; i++)
        {
            int expToAddThisStep = i == steps - 1 ? -1 : stepExp;
            _rewardsUIFeedbackService.PlayRewardFeedback(new RewardFeedbackData
            {
                AnimationTime = 1f,
                Target = UIFeedbackTarget.ExpPoints,
                RewardsCount = 1,
                StartPosition = _uiStartPosition,
                AnimationPointsCount = 4,
                OnComplete = () =>
                {
                    if (expToAddThisStep == -1)
                        _playerData.TotalExpPresented = _playerData.TotalExp;
                    else
                        _playerData.TotalExpPresented = Mathf.Min(_playerData.TotalExpPresented + stepExp, _playerData.TotalExp);
                }
            });
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: token);
        }
    }
}