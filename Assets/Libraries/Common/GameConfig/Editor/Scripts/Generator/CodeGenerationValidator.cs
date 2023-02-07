using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Game.Config.Model
{
    public class CodeGenerationValidator
    {
        
        public static string ValidateColumn(string sheetTitle, Dictionary<int, ConfigColumn> configModelColumns, int i, string cellValue, int j, int columnIndex, Regex alphanumericRegex, int rowIndex)
        {
            switch (i)
            {
                case 0:
                    return GetMemberNameErrors( sheetTitle, cellValue, columnIndex, j, alphanumericRegex);
                case 1:
                    return GetMemberTypeError(sheetTitle, configModelColumns, cellValue, columnIndex, j);
                default:
                    return GetMemberValueError(sheetTitle, configModelColumns, cellValue, j, rowIndex, columnIndex, i);
            }
        }
        
        private static string GetMemberNameErrors(string sheetTitle, string cellValue, int columnIndex, int j, Regex alphanumericRegex)
        {
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                return $"property name with column index {columnIndex} in sheet {sheetTitle} is empty";
            }

            if (j == 0 && cellValue != "id")
            {
                return $"first property name in sheet {sheetTitle} should be 'id' instead of {cellValue}";
            }

            if (char.IsUpper(cellValue[0]))
            {
                return $"property {cellValue} with column index {columnIndex} in sheet {sheetTitle} should start with a lowercase character";
            }

            if (Regex.Replace(cellValue, @"\s+", "") != cellValue)
            {
                return $"property {cellValue} with column index {columnIndex} in sheet {sheetTitle} should not contain white spaces";
            }

            if (!alphanumericRegex.IsMatch(cellValue))
            {
                return $"property {cellValue} with column index {columnIndex} in sheet {sheetTitle} should be alphanumeric";
            }

            return "";
        }
    
        private static string GetMemberTypeError(string sheetTitle, Dictionary<int, ConfigColumn> configModelColumns,
            string cellValue, int columnIndex, int j)
        {
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                return $"type with column index {columnIndex} in sheet {sheetTitle} is empty";
            }

            if (Regex.Replace(cellValue, @"\s+", "") != cellValue)
            {
                return $"type {cellValue} with column index {columnIndex} in sheet {sheetTitle} should not contain white spaces";
            }

            if (j == 0 && cellValue != "string")
            {
                return $"first type in sheet {sheetTitle} should be 'string' instead of {cellValue}";
            }

            if (cellValue != "bool" && cellValue != "bool[]" && cellValue != "int" && cellValue != "int[]" &&
                cellValue != "float" && cellValue != "float[]" && cellValue != "string" &&
                cellValue != "string[]" &&
                !cellValue.StartsWith("string,") && !cellValue.StartsWith("string[],"))
            {
                return $"type with column index {columnIndex} in sheet {sheetTitle} should be 'bool, bool[], int, int[], float, float[], string, string[], string,Type or string[],Type' instead of {cellValue}";
            }

            if (!configModelColumns.ContainsKey(j))
            {
                return $"missing column property name with column index {columnIndex} in sheet {sheetTitle}";
            }

            return "";
        }
    
        private static string GetMemberValueError(string sheetTitle, Dictionary<int, ConfigColumn> configModelColumns,
            string cellValue, int j, int rowIndex, int columnIndex, int i)
        {
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                if (j == 0)
                {
                    return($"missing item id with row index {rowIndex} in sheet {sheetTitle}");
                }
                return "";
            }

            if (!configModelColumns.ContainsKey(j))
            {
                return($"missing column metadata with row index {rowIndex} and column index {columnIndex} in sheet {sheetTitle}");
            
            }

            if (!configModelColumns[j].AddValue(i - 2, cellValue))
            {
                var configModelColumn = configModelColumns[j];
                return($"invalid value {cellValue} with row index {rowIndex} and column index {columnIndex} for property {configModelColumn.Id} with type {configModelColumn.Type} in sheet {sheetTitle}");
            }

            return "";
        }

      
    }
}