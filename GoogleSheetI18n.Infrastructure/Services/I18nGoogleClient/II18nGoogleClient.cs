using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetI18n.Infrastructure.Core;

namespace GoogleSheetI18n.Infrastructure.Services.I18nGoogleClient
{
    public interface II18nGoogleClient
    {
        Task<IList<I18nSheet>> GetSheets(string spreadsheetId);
    }
}