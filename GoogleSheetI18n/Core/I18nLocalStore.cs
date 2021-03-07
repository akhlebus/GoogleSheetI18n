﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleSheetI18n.Api.Core
{
    public class I18nLocalStore : II18nLocalStore
    {
        public readonly string _localStoreFolderPath;

        public I18nLocalStore(string localStoreFolderPath)
        {
            if (localStoreFolderPath is null or "")
            {
                localStoreFolderPath = "local-store";
            }

            _localStoreFolderPath = !string.IsNullOrEmpty(localStoreFolderPath) && !Path.IsPathRooted(localStoreFolderPath)
                ? $@"{AppDomain.CurrentDomain.BaseDirectory}\{localStoreFolderPath}"
                : localStoreFolderPath;

            if (localStoreFolderPath is not null and not "")
            {
                Directory.CreateDirectory(_localStoreFolderPath);
            }
        }

        public async Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName)
        {
            if (spreadsheetId is null or "")
                throw new ArgumentNullException(nameof(spreadsheetId));

            if (sheetName is null or "")
                throw new ArgumentNullException(nameof(sheetName));

            var localStoreKey = GetLocalStoreKey(spreadsheetId, sheetName);
            var localStorePath = Path.Combine(_localStoreFolderPath, localStoreKey);

            if (!File.Exists(localStorePath)) return null;

            var json = await File.ReadAllTextAsync(localStorePath);
            return JsonConvert.DeserializeObject<I18nSheet>(json);
        }

        public async Task SaveSheets(IList<I18nSheet> sheets)
        {
            foreach (var sheet in sheets)
            {
                await SaveSheet(sheet);
            }
        }

        public async Task SaveSheet(I18nSheet sheet)
        {
            var backupKey = GetLocalStoreKey(sheet.SpreadsheetId, sheet.SheetName);
            var backupPath = Path.Combine(_localStoreFolderPath, backupKey);
            var json = JsonConvert.SerializeObject(sheet);
            await File.WriteAllTextAsync(backupPath, json);
        }

        private string GetLocalStoreKey(string spreadsheetId, string sheetName)
        {
            return $"{spreadsheetId}.{sheetName}.json";
        }
    }
}