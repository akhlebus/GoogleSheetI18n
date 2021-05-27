using System.Collections.Generic;
using GoogleSheetI18n.Core.Models.Validation;
using GoogleSheetI18n.Infrastructure.Core;

namespace GoogleSheetI18n.Infrastructure.Validation
{
    public interface II18nCodeTranslation
    {
        string GetExtractedTranslations(string pageName);

        ErrorModel Validate(IList<I18nSheet> i18nSheets);
    }
}
