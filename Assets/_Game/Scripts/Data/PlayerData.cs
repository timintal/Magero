using _Game.DataExtension;
using Newtonsoft.Json;

namespace _Game.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class PlayerData : PersistentDataBase
    {
        public override string DataId => "PlayerData";

        [DataField, JsonProperty] protected int _playerLevel = 1;
        [DataField, JsonProperty] protected int _coins = 0;

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