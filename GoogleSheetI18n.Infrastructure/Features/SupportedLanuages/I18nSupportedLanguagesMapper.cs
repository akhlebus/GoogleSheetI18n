using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Common.Exceptions;

namespace GoogleSheetI18n.Infrastructure.Features.SupportedLanuages
{
    public class I18nSupportedLanguagesMapper
    {
        public I18nLanguageList Map(ValueRange sheet)
        {
            var headerRow = sheet.Values.FirstOrDefault();
            var warnings = new List<string>();

            if (headerRow == null) throw new I18nException("The header row doesn't exist.");

            var supportedLanguages = headerRow.Skip(1).Select(c => c.ToString()).ToArray();

            return new I18nLanguageList(supportedLanguages, warnings);
        }
    }
}