using _Game.DataExtension;
using Newtonsoft.Json;

namespace _Game.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class PlayerData : PersistentDataBase
    {
        public override string DataId => "PlayerData";

        [DataField, JsonProperty] protected int _coins = 0;
        
        [DataField, JsonProperty] protected int _playerLevel = 1;
        [DataField, JsonProperty] protected int _playerExp = 0;
        
        [DataField, JsonProperty] protected int _level = 1;
        [DataField(isUpgrade:true), JsonProperty] protected int _wallLevel = 1;
        [DataField(isUpgrade:true), JsonProperty] protected int _incomeRateLevel = 1;
        [DataField(isUpgrade:true), JsonProperty] protected int _archersNumberLevel = 1;
        [DataField(isUpgrade:true), JsonProperty] protected int _archerDamageLevel = 1;
        
        [DataField, JsonProperty] private WeaponType _leftHandWeapon;
        [DataField, JsonProperty] private WeaponType _rightHandWeapon;
        
        
        public bool SpendCoins(int amount)
        {
            if (_coins < amount)
                return false;

            Coins -= amount;
            return true;
        }
    }
}