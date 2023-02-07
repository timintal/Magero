using System;
using System.Collections.Generic;

namespace Game.Config.Model
{
    public class ConfigColumn
    {
        public int Index { get; private set; }
        public string Id { get; private set; }
        public string Type { get; private set; }
        public string ItemType { get; private set; }
        public string ReferenceType { get; private set; }
        public bool IsArray { get; private set; }
        public bool IsReference { get; private set; }
        public Dictionary<int, object> Values { get; private set; } = new Dictionary<int, object>();

        public ConfigColumn(int index, string id)
        {
            Index = index;
            Id = id;
        }

        public void SetType(string type)
        {
            Type = type;

            if (Type.Contains("[]"))
                IsArray = true;
            if (Type.Contains(","))
            {
                IsReference = true;
                ReferenceType = $"{type.Split(new string[] { "," }, StringSplitOptions.None)[1]}Model";
                ReferenceType = ReferenceType.Replace("[]", string.Empty);
            }

            if (Type.StartsWith("bool"))
                ItemType = "bool";
            else if (Type.StartsWith("int"))
                ItemType = "int";
            else if (Type.StartsWith("float"))
                ItemType = "float";
            else
                ItemType = "string";
        }

        public bool AddValue(int index, string value)
        {
            bool validParse;
            if (!IsArray)
            {
                var typedValue = CodeGenerator.GetTypedValue(ItemType, value, out validParse);
                if (!validParse)
                    return false;

                Values.Add(index, typedValue);
                return true;
            }
            else
            {
                var values = value.Split(new string[] { "," }, StringSplitOptions.None);
                var valueList = new List<object>();
                for (var i = 0; i < values.Length; i++)
                {
                    var valueItem = values[i];
                    var typedValue = CodeGenerator.GetTypedValue(ItemType, valueItem, out validParse);
                    if (!validParse)
                        return false;

                    valueList.Add(typedValue);
                }

                Values.Add(index, valueList.ToArray());
                return true;
            }
        }

        public bool HasValue(int index) => Values.ContainsKey(index);
        public object GetValue(int index) => HasValue(index) ? Values[index] : null;

        public string GetPropertyCode()
        {
            var PropertyId = $"{Id.Substring(0, 1).ToUpper()}{Id.Substring(1)}";
            return $"public {(IsReference ? ReferenceType : ItemType)}{(IsArray ? "[]" : string.Empty)} {PropertyId} => {Id};";
        }

        public string GetFieldCode()
        {
            return $"protected {(IsReference ? ReferenceType : ItemType)}{(IsArray ? "[]" : string.Empty)} {Id};";
        }
    }
}