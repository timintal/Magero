using System.ComponentModel;
using System.Threading;
using _Game.Data;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UIFramework.Runtime;
using UnityEngine;
using VContainer;

public class GeneralCheats
{
    [Inject] private readonly PlayerData _playerData;
    [Inject] private UIFrame _uiFrame;
    [Inject] RewardsUIFeedbackService _rewardsUIFeedbackService;
   
   
    [Category("Progress")] public int ExpToAdd { get; set; }
    [Category("Progress"), UsedImplicitly]
    public void AddExp()
    {
        _playerData.TotalExp += ExpToAdd;
    }
    
    [Category("Progress"), UsedImplicitly]
    public void AddExpAnimated()
    {
        _playerData.TotalExp += ExpToAdd;
        var syncPresentedExpCommand = new SyncPresentedExpCommand(_rewardsUIFeedbackService, _playerData, _uiFrame.UICamera.ViewportToWorldPoint(new Vector3(0.5f, 0.2f)));
        syncPresentedExpCommand.Execute(CancellationToken.None).Forget();
    }
    
    [Category("Economy"), UsedImplicitly]
    public void AddCoins100()
    {
        _playerData.Coins += 100;
    }
    
    [Category("Economy"), UsedImplicitly]
    public void AddCoins1000()
    {
        _playerData.Coins += 1000;
    }
}