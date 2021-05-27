using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Infrastructure.Features.ObjectTreeForLanguage;
using GoogleSheetI18n.Infrastructure.Features.SupportedLanuages;

namespace GoogleSheetI18n.Infrastructure.Core
{
    public class I18nSheet
    {
        private readonly I18nObjectTreeForLanguageMapper _i18nObjectTreeForLanguageMapper;
        private readonly I18nSupportedLanguagesMapper _i18nSupportedLanguagesMapper;

        public I18nSheet(string spreadsheetId, string sheetName, ValueRange valueRange)
        {
            _i18nObjectTreeForLanguageMapper = new I18nObjectTreeForLanguageMapper();
            _i18nSupportedLanguagesMapper = new I18nSupportedLanguagesMapper();

            SpreadsheetId = spreadsheetId;
            SheetName = sheetName;
            ValueRange = valueRange;
        }

        public string SpreadsheetId { get; init; }
        public string SheetName { get; init; }
        public ValueRange ValueRange { get; init; }

        public I18nScope GetTranslations(string language)
        {
            return _i18nObjectTreeForLanguageMapper.Map(ValueRange, language);
        }

        public I18nLanguageList GetSupportedLanguages()
        {
            return _i18nSupportedLanguagesMapper.Map(ValueRange);
        }
    }
}