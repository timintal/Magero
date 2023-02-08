using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace _Game.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class WeaponData : PersistentDataBase
    { 
        public override string DataId => "WeaponData";
    
        [JsonProperty] Dictionary<WeaponType, Dictionary<string, int>> _weaponLevels = new();

        public Action<WeaponType, int> OnWeaponLevelUpdated;

        public void SetWeaponParamLevel(WeaponType type, int level, string paramName)
        {
            IsDirty = true;
            
            if (!_weaponLevels.ContainsKey(type))
            {
                _weaponLevels.Add(type, new Dictionary<string, int>());    
            }

            if (!_weaponLevels[type].ContainsKey(paramName))
            {
                _weaponLevels[type].Add(paramName, level);    
            }
            else
            {
                _weaponLevels[type][paramName] = level;    
            }

            OnWeaponLevelUpdated?.Invoke(type, level);
        }

        public int GetWeaponParamLevel(WeaponType type, string paramName)
        {
            if (!_weaponLevels.ContainsKey(type))
            {
                return 1;
            }

            if (!_weaponLevels[type].ContainsKey(paramName))
            {
                return 1;
            }

            return _weaponLevels[type][paramName];
        }
        
        public void IncreaseWeaponParamLevel(WeaponType type, string paramName)
        {
            SetWeaponParamLevel(type, GetWeaponParamLevel(type, paramName) + 1, paramName);
        }
    }
}