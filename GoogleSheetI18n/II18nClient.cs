using System.Threading.Tasks;

namespace GoogleSheetI18n.Api
{
    public interface II18nClient
    {
        Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName);
    }
}