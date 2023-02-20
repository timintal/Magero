using _Game.Data;
using DG.Tweening;
using EasyTweens;
using Game.Config.Model;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ExpProgressBar : MonoBehaviour, IRewardUIPositionProvider
{
    [SerializeField] Slider _slider;
    [SerializeField] TMPro.TextMeshProUGUI _label;
    [SerializeField] TMPro.TextMeshProUGUI _levelLabel;
    [SerializeField] TweenAnimation _feedbackAnimation;
    
    [Inject] private PlayerData _playerData;
    [Inject] private GameConfig _gameConfig;
    [Inject] ExpService _expService;
    [Inject] private RewardsUIFeedbackService _rewardsUIFeedbackService;
    
    private void Start()
    {
        _playerData.OnTotalExpPresentedChanged += PlayerDataOnOnPlayerExpChanged;
        
        _rewardsUIFeedbackService.RegisterPositionProvider(UIFeedbackTarget.ExpPoints, this);

        UpdateView();
    }

    private void PlayerDataOnOnPlayerExpChanged(int arg1, int arg2)
    {
        UpdateView();
    }
    
    private void UpdateView()
    {
        var playerLevel = _expService.GetPlayerLevel(_playerData.TotalExpPresented, out var currentExp);

        var maxExp = _gameConfig.GetConfigModel<ExpModel>()[IntToString.Get(playerLevel)].Exp;
        _slider.DOKill();
        var endValue = (float)currentExp/maxExp;
        if (!Mathf.Approximately(_slider.value, endValue))
        {
            _feedbackAnimation.PlayForward();
        }
        _slider.DOValue(endValue, 0.3f).SetEase(Ease.OutSine);
        _label.text = $"{currentExp}/{maxExp}";
        _levelLabel.text = $"{playerLevel}";
    }

    void OnDestroy()
    {
        _playerData.OnTotalExpPresentedChanged += PlayerDataOnOnPlayerExpChanged;
        _rewardsUIFeedbackService.UnregisterPositionProvider(UIFeedbackTarget.ExpPoints);
    }

    public Vector3 GetRewardTargetPosition()
    {
        return _levelLabel.transform.position;
    }
}
