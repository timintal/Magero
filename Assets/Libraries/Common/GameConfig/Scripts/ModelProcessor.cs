using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Config.Model
{
    public class ModelProcessor
    {
        private static readonly Dictionary<Type, FieldInfo[]> typeFieldInfoMap = new Dictionary<Type, FieldInfo[]>();
        private static readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>();

        private static readonly Dictionary<Type, bool>
            typeFieldIsConfigModelReferenceMap = new Dictionary<Type, bool>();

        private static readonly Dictionary<string, object> typeFieldReferenceIdMap = new Dictionary<string, object>();
        private static readonly Dictionary<Type, IDictionary> configModel = new Dictionary<Type, IDictionary>();
        private static readonly Type IConfigModelType = typeof(IConfigModel);
        private static readonly Type IConfigModelArrayType = typeof(IConfigModel[]);

        private static bool IsIConfigModelAssignableFromType(Type type) =>
            IConfigModelType.IsAssignableFrom(type) || IConfigModelArrayType.IsAssignableFrom(type);

        public static Dictionary<string, T> GetConfigModel<T>() where T : IConfigModel
        {
            if (configModel != null && configModel.ContainsKey(typeof(T)))
                return configModel[typeof(T)] as Dictionary<string, T>;

            return null;
        }

        public static void InitConfigModelData()
        {
            configModel.Clear();
            typeFieldInfoMap.Clear();
            typeFieldIsConfigModelReferenceMap.Clear();
            typeFieldReferenceIdMap.Clear();
            typeMap.Clear();
        }

        public static string ProcessConfigModelData(string configModelData)
        {
            var error = "";
            if (string.IsNullOrEmpty(configModelData))
                return "No data to process";

            var configModelList = JsonConvert.DeserializeObject<List<object>>(configModelData);
            if (configModelList == null)
                return "Data to process empty or invalid";

            foreach (var configModelItem in configModelList)
            {
                var configModelItemDictionary = (configModelItem as JObject)?.ToObject<Dictionary<string, object>>();
                if (configModelItemDictionary == null)
                {
                    return "ConfigModelItem is not Dictionary<string, object>";
                }

                if (!configModelItemDictionary.ContainsKey(ConfigModelConstants.NameKey) ||
                    !configModelItemDictionary.ContainsKey(ConfigModelConstants.StructureKey) ||
                    !configModelItemDictionary.ContainsKey(ConfigModelConstants.ItemsKey))
                {
                    return "ConfigModelItemDictionary does not contain name, structure or items";
                }

                var configModelItemName = configModelItemDictionary[ConfigModelConstants.NameKey] as string;
                if (string.IsNullOrEmpty(configModelItemName))
                {
                    return "ConfigModelItemName is null or empty";
                }

                var configModelItemStructure = (configModelItemDictionary[ConfigModelConstants.StructureKey] as JObject)?
                    .ToObject<Dictionary<string, object>>();
                var configModelItemData = (configModelItemDictionary[ConfigModelConstants.ItemsKey] as JArray)?
                    .ToObject<List<Dictionary<string, object>>>()
                    .Where(row => row[ConfigModelConstants.IdKey].ToString().StartsWith(ConfigModelConstants.CommentSymbol) == false);

                if (configModelItemStructure == null || configModelItemData == null)
                {
                    return "ConfigModelItemDictionary structure or items are not right type";
                }

                if (!ProcessConfigModelItem(configModelItemName, configModelItemData, ref error))
                {
                    return error;
                }
            }
            return "";
        }

        public static string ValidateModelReferences()
        {
            var error = "";
            foreach (var configModelType in configModel.Keys)
            {
                if (!ProcessConfigModelItemReferences(configModel[configModelType], ref error))
                {
                    return error;
                }
            }
            return "";
        }

        private static bool ProcessConfigModelItemReferences(IDictionary items, ref string error)
        {
            var type = items.GetType().GetGenericArguments()[1];
            if (!typeFieldInfoMap.ContainsKey(type))
                return true;

            var typeFieldInfo = typeFieldInfoMap[type];

            foreach (IConfigModel instance in items.Values)
            {
                if (instance == null)
                    continue;

                foreach (var fieldInfo in typeFieldInfo)
                {
                    var fieldType = fieldInfo.FieldType;

                    if (!typeFieldIsConfigModelReferenceMap[fieldType])
                        continue;

                    var fieldName = fieldInfo.Name;

                    var typeFieldReferenceIdMapKey = $"{type.FullName}{fieldName}{instance.Id}";
                    if (!typeFieldReferenceIdMap.ContainsKey(typeFieldReferenceIdMapKey))
                        continue;

                    var referenceId = typeFieldReferenceIdMap[typeFieldReferenceIdMapKey];
                    object reference = null;

                    if (fieldType.IsArray)
                    {
                        var arrayMemberType = fieldType.GetElementType();
                        var arrayLength = 1;

                        if (referenceId is IList)
                        {
                            var fieldValueAsList = (referenceId as JArray).ToObject<List<string>>();
                            arrayLength = fieldValueAsList.Count;
                        }

                        var arrayInstance = Array.CreateInstance(fieldType.GetElementType(), arrayLength);

                        if (referenceId is IList)
                        {
                            var fieldValueAsList = (referenceId as JArray).ToObject<List<string>>();
                            var l = fieldValueAsList.Count;
                            for (int i = 0; i < l; i++)
                            {
                                var referenceItem = GetConfigModelObject(arrayMemberType.FullName, fieldValueAsList[i]);
                                if (referenceItem == null)
                                {
                                    return ReferenceError(fieldValueAsList[i], fieldName, type.Name, instance.Id, ref error);
                                }

                                arrayInstance.SetValue(referenceItem, i);
                            }
                        }
                        else
                        {
                            var referenceItem = GetConfigModelObject(arrayMemberType.FullName, referenceId.ToString());
                            if (referenceItem == null)
                            {
                                return ReferenceError(referenceId.ToString(), fieldName, type.Name, instance.Id, ref error);
                            }

                            arrayInstance.SetValue(referenceItem, 0);
                        }

                        reference = arrayInstance;
                    }
                    else
                        reference = GetConfigModelObject(fieldType.FullName, referenceId as string);

                    if (reference != null)
                        type.InvokeMember(fieldName,
                            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                            BindingFlags.SetField, Type.DefaultBinder, instance, new object[] { reference });
                    else
                    {
                        return ReferenceError(referenceId.ToString(), fieldName, type.Name, instance.Id, ref error);
                    }
                }
            }

            return true;
        }

        private static bool ReferenceError(string value, string fieldName, string objectType, string rowId, ref string error)
        {
            objectType = objectType.Split('_')[1];
            int.TryParse(rowId, out int actualRow);
            actualRow += 2; // add the header and make it one based like it is in sheets.
            error = $"Missing reference \"{value}\" in Sheet: {objectType} Column: {fieldName} Entry: {rowId}";
            return false;
        }

        private static bool ProcessConfigModelItem(string name, IEnumerable<Dictionary<string, object>> data, ref string error)
        {
            var type = GetType($"Game.Config.Model.{name}Model");
            if (type == null)
            {
                // TODO VNG-100 - sometimes a brand new sheet won't be imported correctly the first time.
                error = $"Game.Config.Model.{name}Model class is missing";
                return false;
            }

            var dictionary = (IDictionary)typeof(Dictionary<,>).MakeGenericType("".GetType(), type)
                .GetConstructor(Type.EmptyTypes).Invoke(null);
            var row = -1;
            foreach (var dataItem in data)
            {
                row++;

                if (!(dataItem is Dictionary<string, object> instanceData))
                {
                    error = $" missing data in {name}, row {row}";
                    return false;
                }

                var instance = Activator.CreateInstance(type) as IConfigModel;

                if (!typeFieldInfoMap.ContainsKey(type))
                    typeFieldInfoMap[type] =
                        type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                var typeFieldInfo = typeFieldInfoMap[type];
                foreach (var fieldInfo in typeFieldInfo)
                {
                    var fieldType = fieldInfo.FieldType;
                    var fieldName = fieldInfo.Name;

                    if (!typeFieldIsConfigModelReferenceMap.ContainsKey(fieldType))
                        typeFieldIsConfigModelReferenceMap[fieldType] = IsIConfigModelAssignableFromType(fieldType);

                    if (instanceData.ContainsKey(fieldName))
                    {
                        var fieldValue = instanceData[fieldName];
                        if (typeFieldIsConfigModelReferenceMap[fieldType])
                        {
                            var typeFieldReferenceIdMapKey = $"{type.FullName}{fieldName}{instance.Id}";
                            typeFieldReferenceIdMap[typeFieldReferenceIdMapKey] = fieldValue;
                            continue;
                        }

                        if (fieldType.IsArray)
                        {
                            var arrayMemberType = fieldType.GetElementType();
                            var arrayLength = 1;

                            if (fieldValue is IList)
                            {
                                var fieldValueAsList = fieldValue as IList;
                                arrayLength = fieldValueAsList.Count;
                            }

                            var arrayInstance = Array.CreateInstance(arrayMemberType, arrayLength);

                            if (fieldValue is IList)
                            {
                                var fieldValueAsList = fieldValue as IList;
                                var l = fieldValueAsList.Count;
                                for (int i = 0; i < l; i++)
                                    arrayInstance.SetValue(
                                        GetConfigModelBasicFieldType(fieldValueAsList[i], arrayMemberType), i);
                            }
                            else
                                arrayInstance.SetValue(GetConfigModelBasicFieldType(fieldValue, arrayMemberType), 0);

                            fieldValue = arrayInstance;
                        }
                        else
                            fieldValue = GetConfigModelBasicFieldType(fieldValue, fieldType);

                        type.InvokeMember(fieldName,
                            BindingFlags.Instance | BindingFlags.SetField | BindingFlags.NonPublic |
                            BindingFlags.Public, Type.DefaultBinder, instance, new object[] { fieldValue });
                    }
                    else
                    {
                        //create an empty array rather than a null if there is no data. 
                        if (fieldType.IsArray)
                        {
                            var arrayMemberType = fieldType.GetElementType();
                            var arrayInstance = Array.CreateInstance(arrayMemberType, 0);
                            type.InvokeMember(fieldName,
                                                     BindingFlags.Instance | BindingFlags.SetField | BindingFlags.NonPublic |
                                                     BindingFlags.Public, Type.DefaultBinder, instance, new object[] { arrayInstance });
                        }
                    }
                }

                if (!string.IsNullOrEmpty(instance.Id))
                {
                    if (dictionary.Contains(instance.Id))
                    {
                        error = $" duplicated item { instance.Id}";
                        return false;
                    }
                    else
                        dictionary.Add(instance.Id, instance);
                }
            }

            configModel[type] = dictionary;

            return true;
        }

        private static IConfigModel GetConfigModelObject(string typeName, string id)
        {
            var type = GetType(typeName);
            if (type == null || string.IsNullOrEmpty(id))
                return null;

            configModel.TryGetValue(type, out IDictionary configModelDictionary);
            if (configModelDictionary != null && configModelDictionary.Contains(id))
                return configModelDictionary[id] as IConfigModel;

            return null;
        }

        public static object GetConfigModelBasicFieldType(object fieldValue, Type fieldType)
        {
            if (Type.GetTypeCode(fieldType) == TypeCode.Boolean)
            {
                return Convert.ToBoolean(fieldValue);
            }

            if (Type.GetTypeCode(fieldType) == TypeCode.Int32)
            {
                // if (Type.GetTypeCode (fieldValue.GetType ()) == TypeCode.Int64)
                return Convert.ToInt32(fieldValue);
            }

            if (Type.GetTypeCode(fieldType) == TypeCode.Single)
            {
                // if (Type.GetTypeCode (fieldValue.GetType ()) == TypeCode.Double)
                return Convert.ToSingle(fieldValue);
            }

            if (Type.GetTypeCode(fieldType) == TypeCode.String)
            {
                return fieldValue.ToString();
            }

            return fieldValue.ToString();
        }

        private static Type GetType(string typeName)
        {
            if (!typeMap.TryGetValue(typeName, out Type type))
            {
                type = Type.GetType(typeName);
                if (type == null)
                    type = Type.GetType($"{typeName}, Autogenerated, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
                if (type == null) 
                    type = Type.GetType($"{typeName}, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
                if (type == null)
                    type = Type.GetType($"{typeName}, Game.Config, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");

                if (type == null)
                {
                    Debug.LogError($"Type {typeName} not found!");
                }
                
                typeMap.Add(typeName, type);
            }

            return type;
        }
    }
}