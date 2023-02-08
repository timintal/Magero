using _Game.Data;
using JetBrains.Annotations;
using Magero.UIFramework;
using TMPro;
using UnityEngine;
using VContainer;

public class CurrencyPanel : UIScreen
{
    [SerializeField] TextMeshProUGUI _coinsLabel;
    
    private PlayerData _playerData;

    [Inject, UsedImplicitly]
    public void SetDependencies(PlayerData playerData)
    {
        _playerData = playerData;
    
        OnCoinsChanged(_playerData.Coins, _playerData.Coins);

        _playerData.OnCoinsChanged += OnCoinsChanged;
    }
    
    private void OnDestroy()
    {
        _playerData.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int prev, int curr)
    {
        _coinsLabel.text = curr.ToString();
    }
}
