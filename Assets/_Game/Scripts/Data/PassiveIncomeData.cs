using System;
using _Game.DataExtension;
using Newtonsoft.Json;

namespace _Game.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class PassiveIncomeData : PersistentDataBase
    {
        public override string DataId => "PassiveIncomeData";
        
        [DataField,JsonProperty] DateTime _lastClaimTime;
    }
}