using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleSheetI18n.Api.Core
{
    public interface II18nLocalStore : II18nSheetProvider
    {
        Task SaveSheets(IList<I18nSheet> sheets);

    Task SaveSheet(I18nSheet sheet);
    }
}