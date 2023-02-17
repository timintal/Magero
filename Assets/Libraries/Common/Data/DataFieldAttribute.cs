using System;

namespace _Game.DataExtension
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DataFieldAttribute : Attribute
    {
        public bool IsDirtySetter;
        public bool IsUpgrade;
        public bool CreatePresentedCounterpart;

        public DataFieldAttribute(bool isDirtySetter = true, bool isUpgrade = false, bool createPresentedCounterpart = false)
        {
            IsDirtySetter = isDirtySetter;
            IsUpgrade = isUpgrade;
            CreatePresentedCounterpart = createPresentedCounterpart;
        }
    }
}