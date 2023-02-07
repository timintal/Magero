using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEditor;

namespace Game.Config.Model
{
    public class SpreadSheetManager
    {
        private readonly CodeGenerator codeGenerator;
        private readonly SheetsService service;
        private readonly string spreadsheetUrl;
        private readonly string spreadsheetName;

        public SpreadSheetManager(string url, string name, SheetsService service)
        {
            codeGenerator = new CodeGenerator();
            this.service = service;
            this.spreadsheetUrl = url;
            this.spreadsheetName = name;
        }

        public void ImportConfigModel(string outputPath, Action<string> errorCallback, Action<string> onNextCallback)
        {
            var spreadsheet = GetSpreadsheet();
            List<string> sheetTitles = new List<string>();
            foreach (var sheet in spreadsheet.Sheets)
            {
                if (!sheet.Properties.Title.StartsWith(ConfigModelConstants.CommentSymbol))
                {
                    sheetTitles.Add(sheet.Properties.Title);
                }
            }

            var sheetJsonDataMap = new Dictionary<string, string>();
            var configModelColumns = new Dictionary<int, ConfigColumn>();
            var skipColumns = new List<int>();
            for (var index = 0; index < sheetTitles.Count; index++)
            {
                var sheetTitle = sheetTitles[index];
                var prop = (float)(index + 1) / sheetTitles.Count;
                EditorUtility.DisplayProgressBar("Importing...", "importing sheet:" + sheetTitle, prop);
                var data = GetSheetValues(sheetTitle);
                var error = codeGenerator.GenerateCode(data, sheetTitle, configModelColumns, skipColumns, sheetJsonDataMap);
                if (!string.IsNullOrEmpty(error))
                {
                    errorCallback(error);
                    return;
                }
            }

            foreach (var sheetTitle in sheetJsonDataMap.Keys)
            {
                Directory.CreateDirectory($"{outputPath}/{spreadsheetName}");
                File.WriteAllText($"{outputPath}/{spreadsheetName}/{sheetTitle.ToLower()}.json", sheetJsonDataMap[sheetTitle]);
            }

            onNextCallback.Invoke(spreadsheetName);
        }


        private Spreadsheet GetSpreadsheet()
        {
            var request = service.Spreadsheets.Get(spreadsheetUrl);
            var spreadsheet = request.Execute();
            return spreadsheet;
        }

        private IList<IList<object>> GetSheetValues(string sheetTitle)
        {
            var request = service.Spreadsheets.Values.Get(spreadsheetUrl, $"{sheetTitle}!A1:ZZ");
            request.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.UNFORMATTEDVALUE;
            var data = request.Execute();
            var values = data.Values;
            return values;
        }

        public static string GetIdFromURL(string url)
        {
            if (url.Contains("google.com"))
            {
                // we need just the spreadsheet ID. depending on how much is copied we need to remove a certain part 
                // google format says it's the part after the /d/
                var parts = url.Split('/');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "d")
                    {
                        return parts[i + 1];
                    }

                }
            }
            return url;
        }
    }
}
