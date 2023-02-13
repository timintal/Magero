using System;
using _Game.Data;
using Game.Config.Model;
using UnityEngine;

public class PassiveIncomeService 
{
    private readonly PlayerData _playerData;
    private readonly PassiveIncomeData _passiveIncomeData;
    private readonly GameConfig _gameConfig;

    public PassiveIncomeService(PlayerData playerData, PassiveIncomeData passiveIncomeData, GameConfig gameConfig)
    {
        _playerData = playerData;
        _passiveIncomeData = passiveIncomeData;
        _gameConfig = gameConfig;
    }
    
    public int GetPassiveIncome()
    {
        var gameSettingsModels = _gameConfig.GetConfigModel<CastleStatsModel>()[_playerData.IncomeRateLevel.ToString()];
        var passiveIncomeRate = gameSettingsModels.IncomeRate;
        var incomeCapacity = gameSettingsModels.IncomeCapacity;

        float totalMinutes = (float)DateTime.UtcNow.Subtract(_passiveIncomeData.LastClaimTime).TotalMinutes;

        int income = Mathf.RoundToInt(totalMinutes * passiveIncomeRate);
        income = Mathf.Min(income, (int)incomeCapacity);
        
        return income;
    }

    public void ClaimIncome()
    {   
        var income = GetPassiveIncome();
        _playerData.Coins += income;
        _passiveIncomeData.LastClaimTime = DateTime.UtcNow;
    }
}
