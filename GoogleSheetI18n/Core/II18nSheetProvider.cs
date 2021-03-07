using System.Threading.Tasks;

namespace GoogleSheetI18n.Api.Core
{
    public interface II18nSheetProvider
    {
        Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName);
    }
}