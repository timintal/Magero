using System;
using _Game.Data;
using Game.Config.Model;
using VContainer.Unity;

public class ExpService : IDisposable, IStartable
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

    public void Start()
    {
        _playerData.OnPlayerParamUpgraded += PlayerDataOnOnPlayerParamUpgraded;
        _weaponData.OnWeaponLevelUpdated += OnWeaponLevelUpdated;
        
        _playerData.TotalExpPresented = _playerData.TotalExp;
    }

    public int PlayerLevel => GetPlayerLevel(_playerData.TotalExp, out _);
    public int PlayerLevelPresented => GetPlayerLevel(_playerData.TotalExpPresented, out _);
    
    public int GetPlayerLevel(int exp, out int currentExp)
    {
        int ExpForLevel(int i) => _gameConfig.GetConfigModel<ExpModel>()[IntToString.Get(i)].Exp;

        int totalExp = exp;
        int level = 1;

        while (totalExp > ExpForLevel(level))
        {
            totalExp -= ExpForLevel(level);
            level++;
        }

        currentExp = totalExp;
        
        return level;
    }

    private void OnWeaponLevelUpdated(WeaponType arg1, int arg2)
    {
        var gameSettingsModels = _gameConfig.GetConfigModel<GameSettingsModel>()["default"];
        var expForUpgrade = gameSettingsModels.ExpForUpgrade;
        _playerData.TotalExp += (expForUpgrade);
    }

    private void PlayerDataOnOnPlayerParamUpgraded()
    {
        var gameSettingsModels = _gameConfig.GetConfigModel<GameSettingsModel>()["default"];
        var expForUpgrade = gameSettingsModels.ExpForUpgrade;
        _playerData.TotalExp += (expForUpgrade);
    }


    public void Dispose()
    {
        _playerData.OnPlayerParamUpgraded -= PlayerDataOnOnPlayerParamUpgraded;
        _weaponData.OnWeaponLevelUpdated -= OnWeaponLevelUpdated;
    }
}