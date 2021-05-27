using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetI18n.Infrastructure.Core;
using GoogleSheetI18n.Infrastructure.Services.I18nLocalStore;

namespace GoogleSheetI18n.Infrastructure.Caching
{
    public class I18nCachableLocalStore : II18nLocalStore
    {
        private readonly I18nCache _i18nCache;
        private readonly II18nLocalStore _i18nLocalStore;

        public I18nCachableLocalStore(II18nLocalStore localStore, I18nCache i18nCache)
        {
            _i18nLocalStore = localStore;
            _i18nCache = i18nCache;
        }

        public async Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName)
        {
            var cacheKey = GetCacheKey(spreadsheetId, sheetName);

            Task<I18nSheet> AddItem(string key)
            {
                return _i18nLocalStore.GetSheet(spreadsheetId, sheetName);
            }

            return await _i18nCache.GetOrAdd(cacheKey, AddItem);
        }

        public async Task SaveSheets(IList<I18nSheet> sheets)
        {
            await _i18nLocalStore.SaveSheets(sheets);

            foreach (var sheet in sheets)
            {
                var cacheKey = GetCacheKey(sheet.SpreadsheetId, sheet.SheetName);
                _i18nCache.Set(cacheKey, sheet);
            }
        }

        public async Task SaveSheet(I18nSheet sheet)
        {
            await _i18nLocalStore.SaveSheet(sheet);

            var cacheKey = GetCacheKey(sheet.SpreadsheetId, sheet.SheetName);
            _i18nCache.Set(cacheKey, sheet);
        }

        public async Task<IList<I18nSheet>> GetSheets(string spreadsheetId)
        {
            return await _i18nLocalStore.GetSheets(spreadsheetId);
        }

        private string GetCacheKey(string spreadsheetId, string sheetName)
        {
            return $"{spreadsheetId}.{sheetName}";
        }
    }
}