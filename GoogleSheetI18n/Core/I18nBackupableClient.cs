using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetI18n.Api.Exceptions;

namespace GoogleSheetI18n.Api.Core
{
    public class I18nBackupableClient : II18nClient
    {
        private static readonly Dictionary<string, I18nSheet> _cache = new();
        private readonly I18nBackup _i18NBackup;

        private readonly II18nClient _i18nClient;

        public I18nBackupableClient(II18nClient i18nClient, I18nBackup backup)
        {
            _i18nClient = i18nClient;
            _i18NBackup = backup;
        }

        public async Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName)
        {
            try
            {
                var i18nSheet = await _i18nClient.GetSheet(spreadsheetId, sheetName);

                return i18nSheet;
            }
            catch (I18nException e)
            {
                var i18nSheet = await _i18NBackup.GetSheet(spreadsheetId, sheetName);
                return i18nSheet;
            }
        }

        private string GetCacheKey(string spreadsheetId, string sheetName)
        {
            return $"{spreadsheetId}.{sheetName}";
        }
    }
}