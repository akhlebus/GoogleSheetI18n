using System.Collections.Generic;
using GoogleSheetI18n.Api.Core;

namespace GoogleSheetI18n.Api.Validation
{
    public interface II18nCodeTranslation
    {
        string GetExtractedTranslations(string pageName);

        ErrorModel Validate(IList<I18nSheet> i18nSheets);
    }
}
