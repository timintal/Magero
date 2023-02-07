using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace Game.Config.Model
{
    public class GoogleSheetService
    {
        public static SheetsService GetService(string credentialFileUri)
        {
            GoogleCredential credential;
            string[] scopes = { SheetsService.Scope.Spreadsheets };
            try
            {
                using (var stream = new FileStream(credentialFileUri, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
                }

                return new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "game"
                });
            }
            catch (Exception)
            {
                throw new Exception("authentication error");
            }
        }

        public static string ProcessGoogleSpreadsheetManagerException(Exception e)
        {

            if (e.Message == "authentication error" || e.Message.Contains("Not a valid email or user ID"))
                return "Authentication error, please check Google credentials";
            if (e.Message.Contains("Requested entity was not found"))
                return "Invalid spreadsheet id";
            return e.Message;
        }
    }
}