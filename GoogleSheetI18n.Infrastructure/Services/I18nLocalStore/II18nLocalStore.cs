using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetI18n.Infrastructure.Core;

namespace GoogleSheetI18n.Infrastructure.Services.I18nLocalStore
{
    public interface II18nLocalStore : II18nSheetProvider
    {
        Task SaveSheets(IList<I18nSheet> sheets);

        Task SaveSheet(I18nSheet sheet);
    }
}