using _Game.Data;
using JetBrains.Annotations;
using UIFramework;
using TMPro;
using UnityEngine;
using VContainer;

public class CurrencyPanel : UIScreen, IRewardUIPositionProvider
{
    [SerializeField] TextMeshProUGUI _coinsLabel;
    [SerializeField] private RectTransform _coinsIcon;
    
    private PlayerData _playerData;
    private RewardsUIFeedbackService _rewardsUIFeedbackService;

    [Inject, UsedImplicitly]
    public void SetDependencies(PlayerData playerData, RewardsUIFeedbackService rewardsUIFeedbackService)
    {
        _rewardsUIFeedbackService = rewardsUIFeedbackService;
        _playerData = playerData;
    
        OnCoinsChanged(_playerData.Coins, _playerData.Coins);

        _playerData.OnCoinsChanged += OnCoinsChanged;
    }

    protected override void OnOpening()
    {
        _rewardsUIFeedbackService.RegisterPositionProvider(UIFeedbackTarget.Coins, this);
    }
    
    protected override void OnClosing()
    {
        _rewardsUIFeedbackService.UnregisterPositionProvider(UIFeedbackTarget.Coins);
    }

    private void OnDestroy()
    {
        _playerData.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int prev, int curr)
    {
        _coinsLabel.text = curr.ToString();
    }

    public Vector3 GetRewardTargetPosition()
    {
        return _coinsIcon.position;
    }
}
