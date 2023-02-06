using _Game.Data;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using VContainer;

public class LevelLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    
    private PlayerData _playerData;

    private void OnPlayerLevelChanged(int prev, int curr)
    {
        _label.text = $"LEVEL {curr}";
    }

    void OnDestroy()
    {
        _playerData.OnPlayerLevelChanged -= OnPlayerLevelChanged;
    }
    
    [Inject, UsedImplicitly]
    public void SetDependencies(PlayerData playerData)
    {
        _playerData = playerData;
        
        _playerData.OnPlayerLevelChanged += OnPlayerLevelChanged;
        OnPlayerLevelChanged(_playerData.PlayerLevel, _playerData.PlayerLevel);
    }

}
