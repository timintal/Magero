using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace _Game.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class WeaponData : PersistentDataBase
    { 
        public override string DataId => "WeaponData";
    
        [JsonProperty] Dictionary<WeaponType, int> _weaponLevels = new();

        public Action<WeaponType, int> OnWeaponLevelUpdated;

        public void SetWeaponLevel(WeaponType type, int level)
        {
            IsDirty = true;
            
            if (!_weaponLevels.ContainsKey(type))
            {
                _weaponLevels.Add(type, 1);    
            }
            
            _weaponLevels[type] = level;
            OnWeaponLevelUpdated?.Invoke(type, level);
        }

        public void UpgradeWeaponLevel(WeaponType type)
        {
            if (type == WeaponType.None) return;
            
            IsDirty = true;
            if (!_weaponLevels.ContainsKey(type))
            {
                _weaponLevels.Add(type, 1);    
            }

            _weaponLevels[type] += 1;
            OnWeaponLevelUpdated?.Invoke(type, _weaponLevels[type]);
        }

        public int GetWeaponLevel(WeaponType type)
        {
            if (!_weaponLevels.ContainsKey(type))
            {
                return 1;
            }

            return _weaponLevels[type];
        }
    }
}