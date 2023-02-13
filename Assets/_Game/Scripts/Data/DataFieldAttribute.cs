using System;

namespace _Game.DataExtension
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DataFieldAttribute : Attribute
    {
        public bool IsDirtySetter;
        public bool IsUpgrade;

        public DataFieldAttribute(bool isDirtySetter = true, bool isUpgrade = false)
        {
            IsDirtySetter = isDirtySetter;
            IsUpgrade = isUpgrade;
        }
    }
}