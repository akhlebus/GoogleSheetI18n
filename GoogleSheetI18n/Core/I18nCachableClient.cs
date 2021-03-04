using System.Threading.Tasks;

namespace GoogleSheetI18n.Api.Core
{
    public class I18nCachableClient : II18nClient
    {
        private readonly I18nCache _i18nCache;
        private readonly II18nClient _i18nClient;

        public I18nCachableClient(II18nClient i18nClient, I18nCache i18nCache)
        {
            _i18nClient = i18nClient;
            _i18nCache = i18nCache;
        }

        public async Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName)
        {
            var cacheKey = GetCacheKey(spreadsheetId, sheetName);

            Task<I18nSheet> AddItem(string key)
            {
                return _i18nClient.GetSheet(spreadsheetId, sheetName);
            }

            return await _i18nCache.GetOrAdd(cacheKey, AddItem);
        }

        private string GetCacheKey(string spreadsheetId, string sheetName)
        {
            return $"{spreadsheetId}.{sheetName}";
        }
    }
}