using System;
using _Game.Data;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using VContainer;

public class PlayerLevelLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    
    private PlayerData _playerData;
    
    [Inject] ExpService _expService;

    private void Start()
    {
        var playerLevel = _expService.GetPlayerLevel(_playerData.TotalExpPresented, out _);
        OnPlayerLevelChanged(playerLevel, playerLevel);
    }

    private void OnPlayerLevelChanged(int prev, int curr)
    {
        _label.text = $"MAGE LEVEL {curr}";
    }

    void OnDestroy()
    {
        _playerData.OnTotalExpChanged -= OnPlayerLevelChanged;
    }
    
    [Inject, UsedImplicitly]
    public void SetDependencies(PlayerData playerData)
    {
        _playerData = playerData;
        
        _playerData.OnTotalExpChanged += OnPlayerLevelChanged;
    }

}