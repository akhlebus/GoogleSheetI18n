using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleSheetI18n.Api.Core
{
    public interface II18nGoogleClient
    {
        Task<IList<I18nSheet>> GetSheets(string spreadsheetId);
    }
}