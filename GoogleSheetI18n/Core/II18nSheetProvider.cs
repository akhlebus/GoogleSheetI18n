using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleSheetI18n.Api.Core
{
    public interface II18nSheetProvider
    {
        Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName);
        Task<IList<I18nSheet>> GetSheets(string spreadsheetId);
    }
}