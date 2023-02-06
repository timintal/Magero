using System;

namespace _Game.DataExtension
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DataFieldAttribute : Attribute
    {
        public bool IsDirtySetter;

        public DataFieldAttribute(bool isDirtySetter = true)
        {
            IsDirtySetter = isDirtySetter;
        }
    }
}