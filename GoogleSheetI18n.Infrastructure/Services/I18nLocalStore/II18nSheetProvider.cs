using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetI18n.Infrastructure.Core;

namespace GoogleSheetI18n.Infrastructure.Services.I18nLocalStore
{
    public interface II18nSheetProvider
    {
        Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName);
        Task<IList<I18nSheet>> GetSheets(string spreadsheetId);
    }
}