using System;
using _Game.Data;
using Game.Config.Model;

public class ExpForUpgradeService: IDisposable
{
    private readonly PlayerData _playerData;
    private readonly WeaponData _weaponData;
    private readonly GameConfig _gameConfig;

    public ExpForUpgradeService(PlayerData playerData, WeaponData weaponData, GameConfig gameConfig)
    {
        _playerData = playerData;
        _weaponData = weaponData;
        _gameConfig = gameConfig;
    }
    
    public void Init()
    {
        _playerData.OnPlayerParamUpgraded += PlayerDataOnOnPlayerParamUpgraded;
        _weaponData.OnWeaponLevelUpdated += OnWeaponLevelUpdated;
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

    public void Dispose()
    {
        _playerData.OnPlayerParamUpgraded -= PlayerDataOnOnPlayerParamUpgraded;
        _weaponData.OnWeaponLevelUpdated -= OnWeaponLevelUpdated;
    }
}