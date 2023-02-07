using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Config.Model
{
    [Serializable]
    public class ConfigSheet
    {
        public string ID;
        public string URL;
        public bool LoadByDefault;
    }

    [Serializable]
    public class ConfigData : ScriptableObject
    {
        public string GoogleSpreadsheetCredentialFileUri;

        public bool Debug_UseLocalConfigModel;
        public bool Debug_AlwaysDownloadFromCDN;
        public bool Debug_TestProdCDN;
        public List<ConfigSheet> Sheets = new List<ConfigSheet>();
        private Validator _validator;
        public Validator Validator => _validator;

        // Needed by the final game.
        public string S3Path;
        public int Version;

        public List<string> GetDefaultSheets()
        {
            List<string> ret = new List<string>();
            foreach (var s in Sheets)
            {
                if (s.LoadByDefault)
                {
                    ret.Add(s.ID);
                }
            }
            return ret;
        }

        public string GetCachedPath()
        {
            return Path.Combine(Application.persistentDataPath, "Config/Cache", Version.ToString());
        }

        public string GetCachedIndexFile()
        {
            return Path.Combine(GetCachedPath(), ConfigPaths.IndexFileName);
        }

        public string GetS3Path()
        {
            string env = "dev";
            if (!Debug.isDebugBuild || Debug_TestProdCDN)
            {
                env = "prod";
            }
            return $"{S3Path}/{env}/{Version}/";
        }

        public string GetS3IndexFile()
        {
            return $"{GetS3Path()}{ConfigPaths.IndexFileName}";
        }
    }


}
