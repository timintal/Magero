using System;
using _Game.Data;
using Game.Config.Model;

public class ExpService : IDisposable
{
    private readonly PlayerData _playerData;
    private readonly WeaponData _weaponData;
    private readonly GameConfig _gameConfig;

    public ExpService(PlayerData playerData, WeaponData weaponData, GameConfig gameConfig)
    {
        _playerData = playerData;
        _weaponData = weaponData;
        _gameConfig = gameConfig;
    }
    
    public void Init()
    {
        _playerData.OnPlayerParamUpgraded += PlayerDataOnOnPlayerParamUpgraded;
        _weaponData.OnWeaponLevelUpdated += OnWeaponLevelUpdated;
        _playerData.OnPlayerExpChanged += PlayerDataOnOnPlayerExpChanged;
        
        UpdatePlayerLevel();
    }

    private void PlayerDataOnOnPlayerExpChanged(int previous, int current)
    {
        if (current > previous)
            UpdatePlayerLevel();
    }

    private void OnWeaponLevelUpdated(WeaponType arg1, int arg2)
    {
        var gameSettingsModels = _gameConfig.GetConfigModel<GameSettingsModel>()["default"];
        var expForUpgrade = gameSettingsModels.ExpForUpgrade;
        _playerData.PlayerExp += (expForUpgrade);
        
    }

    private void PlayerDataOnOnPlayerParamUpgraded()
    {
        var gameSettingsModels = _gameConfig.GetConfigModel<GameSettingsModel>()["default"];
        var expForUpgrade = gameSettingsModels.ExpForUpgrade;
        _playerData.PlayerExp += (expForUpgrade);
    }
    
    void UpdatePlayerLevel()
    {
        var expModels = _gameConfig.GetConfigModel<ExpModel>();
        
        var expToNextLevel = expModels[_playerData.PlayerLevel.ToString()];

        while (_playerData.PlayerExp >= expToNextLevel.Exp)
        {
            _playerData.PlayerExp -= expToNextLevel.Exp;
            _playerData.PlayerLevel += 1;
            expToNextLevel = expModels[_playerData.PlayerLevel.ToString()];
        }
    }
    

    public void Dispose()
    {
        _playerData.OnPlayerParamUpgraded -= PlayerDataOnOnPlayerParamUpgraded;
        _weaponData.OnWeaponLevelUpdated -= OnWeaponLevelUpdated;
    }
}