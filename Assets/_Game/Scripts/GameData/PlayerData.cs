
using _Game.DataExtension;
using Newtonsoft.Json;

namespace _Game.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class PlayerData : PersistentDataBase
    {
        public override string DataId => "PlayerData";

        [DataField, JsonProperty] protected int _playerLevel = 0;
        
    }
}