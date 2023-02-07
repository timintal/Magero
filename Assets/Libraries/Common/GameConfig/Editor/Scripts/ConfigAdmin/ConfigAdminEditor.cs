using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Game.Config.Model
{
    public class ConfigAdminEditor : EditorWindow
    {
        private static readonly string ConfigModelPath = "Assets/Resources/GameConfig";
        private static readonly string ConfigModelUri = $"{ConfigModelPath}/ConfigData.asset";

        private ConfigData _data = null;
        private ConfigAdminEditorImporter _importer = null;
        private Vector2 _scrollPosition;
        private bool willRefreshAssetDatabase = false;
        // private int _editorTab = 0;

        public ConfigData Data => _data;

        [MenuItem("Tools/Game Config/Settings")]
        static public void Open()
        {
            var configAdmin = GetWindow(typeof(ConfigAdminEditor)) as ConfigAdminEditor;
            configAdmin.titleContent = new GUIContent("Config Settings");
        }

        private void OnEnable()
        {
            if (_importer == null)
            {
                _importer = new ConfigAdminEditorImporter(this);
            }

            InitializeModel();
        }

        private void Update()
        {
            if (willRefreshAssetDatabase)
            {
                willRefreshAssetDatabase = false;
                AssetDatabase.Refresh();
            }
        }

        private void InitializeModel()
        {
            if (_data == null)
            {
                _data = (ConfigData)AssetDatabase.LoadAssetAtPath(ConfigModelUri, typeof(ConfigData));
                if (_data == null)
                {
                    Directory.CreateDirectory(ConfigModelPath);
                    _data = CreateInstance<ConfigData>();
                    var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(ConfigModelUri);
                    AssetDatabase.CreateAsset(_data, assetPathAndName);
                    AssetDatabase.SaveAssets();
                }
            }
        }

        private void OnGUI()
        {
            if (_data == null)
                return;

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.Space();
            DrawGeneralTab();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.Label("                              SHEETS", EditorStyles.boldLabel);
            DrawSheetsTab();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.Label("                              SETTINGS", EditorStyles.boldLabel);
            DrawSettingsTab();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            GUI.color = Color.green;
            if (GUILayout.Button("Import All"))
            {
                OnImportButton();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            EditorGUILayout.EndScrollView();
            GUI.color = Color.white;
            if (GUI.changed)
                EditorUtility.SetDirty(_data);
        }

        private void DrawGeneralTab()
        {
            EditorGUILayout.Space();
            GUILayout.Label("                              EDITOR DEBUG", EditorStyles.boldLabel);

            _data.Debug_UseLocalConfigModel = EditorGUILayout.Toggle("Use local config", _data.Debug_UseLocalConfigModel);
            _data.Debug_AlwaysDownloadFromCDN = EditorGUILayout.Toggle("Always pull from CDN", _data.Debug_AlwaysDownloadFromCDN);
            _data.Debug_TestProdCDN = EditorGUILayout.Toggle("Test Prod CDN", _data.Debug_TestProdCDN);

            EditorGUILayout.Space();
            GUILayout.Label("                              RUNTIME", EditorStyles.boldLabel);

            _data.S3Path = EditorGUILayout.TextField("S3 URL", _data.S3Path);
            GUILayout.BeginHorizontal();

            _data.Version = EditorGUILayout.IntField("Version", _data.Version);
            if (GUILayout.Button("Test S3 files"))
            {
                Application.OpenURL(_data.GetS3IndexFile());
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void OnImportButton(List<ConfigSheet> sheets=null)
        {
            _importer.Import(sheets);
            EditorUtility.ClearProgressBar();
            var configModel = new GameConfig();
            configModel.Init(() =>
            {
                bool success = ValidateModel(configModel);
                if (success)
                {
                    _data.Version += 1;
                    EditorUtility.SetDirty(_data);
                }
            });
        }

        private bool ValidateModel(GameConfig config)
        {
            var msg = Validator.EverythingIsOkMsg;

            if (_data.Validator != null)
            {
                msg = _data.Validator.ValidateModel(config);
            }

            EditorUtility.DisplayDialog("Validation", msg, "ok");

            return msg.Equals(Validator.EverythingIsOkMsg);
        }

        private void DrawSheetsTab()
        {
            if (GUILayout.Button("Add Spreadsheet"))
                _data.Sheets.Add(new ConfigSheet());
            foreach (var sheet in _data.Sheets)
            {
                GUILayout.BeginHorizontal();
                sheet.ID = EditorGUILayout.TextField("ID", sheet.ID);
                if (GUILayout.Button("Open...", GUILayout.Width(100)))
                {
                    Application.OpenURL("https://docs.google.com/spreadsheets/d/" + sheet.URL);
                }
                GUI.color = Color.yellow;
                if (GUILayout.Button("Import this",GUILayout.Width(120)))
                {
                    OnImportButton(new List<ConfigSheet>(){sheet});
                    return;
                }
                GUI.color = Color.red;
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_TreeEditor.Trash"), GUILayout.Width(32), GUILayout.Height(22)))
                {
                    if (EditorUtility.DisplayDialog("Do you want to continue?", "Delete " + sheet.ID, "Yes", "Cancel"))
                    {
                        _data.Sheets.Remove(sheet);
                        return;
                    }
                }

                GUI.color = Color.white;
                GUILayout.EndHorizontal();
                sheet.LoadByDefault = EditorGUILayout.Toggle("Load by default", sheet.LoadByDefault);
                var newUrl = EditorGUILayout.TextField("Spreadsheet URL", sheet.URL);
                if (newUrl != sheet.URL)
                {
                    // see if we need to fix up the URL?
                    sheet.URL = SpreadSheetManager.GetIdFromURL(newUrl);
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }

        private void DrawSettingsTab()
        {
            GUILayout.BeginHorizontal();
            _data.GoogleSpreadsheetCredentialFileUri =
                EditorGUILayout.TextField("Google credential file", _data.GoogleSpreadsheetCredentialFileUri);
            if (GUILayout.Button("Select", GUILayout.Width(100f)))
            {
                var filename = EditorUtility.OpenFilePanel("Select credential file", Path.GetDirectoryName(Application.dataPath), string.Empty);
                if (!string.IsNullOrEmpty(filename))
                {
                    // try to convert to a relative path so it's cross computer happy (would be good to move this to some file utils).
                    string projectPath = Path.GetDirectoryName(Application.dataPath).Replace('\\', '/');
                    _data.GoogleSpreadsheetCredentialFileUri = "." + filename.Replace(projectPath, "");
                }
                EditorUtility.SetDirty(_data);
            }

            if (GUILayout.Button("Help", GUILayout.Width(50f)))
                ShowGoogleCredentialHelp();
            GUILayout.EndHorizontal();

        }
        
        public void MarkAssetDatabaseForRefresh()
        {
            willRefreshAssetDatabase = true;
        }

        private void ShowGoogleCredentialHelp()
        {
            string url = "https://console.developers.google.com/apis/dashboard";
            var ret = ShowDialog(
                "1. Go to " + url + @" and navigate to the API section. You should see a dashboard.
2. Click on  “Enable APIs” or “Library” which should take you to the library of services that you can connect to. Search and enable the Google Sheets API.
3. Go to Credentials and select “Create credentials”.
4. Select “Service Account” and proceed forward by creating this service account. It can be named whatever you want.
5. Under “Role”, select Project > Owner or Editor, depending on what level of access you want to grant.
6. Select JSON as the Key Type and click “Create”. This should automatically download a JSON file with your credentials.
7. Rename this credentials file as client_secret.json and copy it into your working directory.
8. The final administrative step is super important! Take the “client email” that is in your credentials file and grant access to that particular email in the sheet that you’re working in. You can do this by clicking “Share” in the top left of your spreadsheet and then pasting that email in the field, enabling with “Can edit”. If you do not do this, you will get an error when trying to pull the data."
                , "Copy URL to Clipboard");

            if (!ret)
            {
                EditorGUIUtility.systemCopyBuffer = url;
            }
        }

        public bool ShowDialog(string error, string cancelButton = null)
        {
            return EditorUtility.DisplayDialog("Settings", error, "ok", cancelButton);
        }
    }
}