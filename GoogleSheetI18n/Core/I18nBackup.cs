using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleSheetI18n.Api.Core
{
    public class I18nBackup
    {
        public readonly string _backupsFolderPath;

        public I18nBackup(string backupsFolderPath)
        {
            _backupsFolderPath = !string.IsNullOrEmpty(backupsFolderPath) && !Path.IsPathRooted(backupsFolderPath)
                ? $@"{Environment.CurrentDirectory}\{backupsFolderPath}"
                : backupsFolderPath;

            if (backupsFolderPath is not null and not "")
            {
                Directory.CreateDirectory(_backupsFolderPath);
            }
        }

        public async Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName)
        {
            var backupKey = GetBackupKey(spreadsheetId, sheetName);
            var backupPath = Path.Combine(_backupsFolderPath, backupKey);

            if (!File.Exists(backupPath)) return null;

            var json = await File.ReadAllTextAsync(backupPath);
            return JsonConvert.DeserializeObject<I18nSheet>(json);
        }

        public async Task SaveSheet(I18nSheet sheet)
        {
            var backupKey = GetBackupKey(sheet.SpreadsheetId, sheet.SheetName);
            var backupPath = Path.Combine(_backupsFolderPath, backupKey);
            var json = JsonConvert.SerializeObject(sheet);
            await File.WriteAllTextAsync(backupPath, json);
        }

        private string GetBackupKey(string spreadsheetId, string sheetName)
        {
            return $"{spreadsheetId}.{sheetName}.json";
        }
    }
}