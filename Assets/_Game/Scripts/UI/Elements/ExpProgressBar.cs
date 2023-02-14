using _Game.Data;
using Game.Config.Model;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ExpProgressBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TMPro.TextMeshProUGUI _label;
    [SerializeField] TMPro.TextMeshProUGUI _levelLabel;
    
    [Inject] private PlayerData _playerData;
    [Inject] private GameConfig _gameConfig;
    
    private void Start()
    {
        _playerData.OnPlayerExpChanged += PlayerDataOnOnPlayerExpChanged;
        _playerData.OnPlayerLevelChanged += PlayerDataOnOnPlayerExpChanged;
        UpdateView();
    }

    private void PlayerDataOnOnPlayerExpChanged(int arg1, int arg2)
    {
        UpdateView();
    }
    
    private void UpdateView()
    {
        var currExp = _playerData.PlayerExp;
        var maxExp = _gameConfig.GetConfigModel<ExpModel>()[_playerData.PlayerLevel.ToString()].Exp;
        _slider.value = (float)currExp/maxExp;
        _label.text = $"{currExp}/{maxExp}";
        _levelLabel.text = $"{_playerData.PlayerLevel}";
    }
    
    void OnDestroy()
    {
        _playerData.OnPlayerExpChanged -= PlayerDataOnOnPlayerExpChanged;
    }
}
