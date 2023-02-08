using System;
using Game.Config.Model;

public class UpgradableWeaponParam
{
    public string ParamName;
    public string ParamKey;
    public Func<SpellsStatsModel, float> GetParamValue;
    
    public float GetParamValueForLevel(int level, GameConfig config)
    {
        if (!config.GetConfigModel<SpellsStatsModel>().ContainsKey(level.ToString()))
        {
            return -1;
        }
        
        return GetParamValue(config.GetConfigModel<SpellsStatsModel>()[level.ToString()]);
    }
}