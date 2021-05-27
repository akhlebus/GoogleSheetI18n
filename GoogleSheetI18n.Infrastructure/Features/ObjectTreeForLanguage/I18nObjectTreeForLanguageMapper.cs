using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Common.Exceptions;
using GoogleSheetI18n.Common.Extensions;

namespace GoogleSheetI18n.Infrastructure.Features.ObjectTreeForLanguage
{
    public class I18nObjectTreeForLanguageMapper
    {
        public I18nScope Map(ValueRange sheet, string language)
        {
            var (keyIndex, valueIndex) = MapHeader(sheet.Values.FirstOrDefault(), language);
            var scope = MapBody(sheet.Values.Skip(1).ToArray(), keyIndex, valueIndex);

            return scope;
        }

        private (int KeyIndex, int ValueIndex) MapHeader(IList<object> headerRow, string language)
        {
            if (headerRow == null) throw new I18nException("The header row doesn't exist.");

            var keyIndex = headerRow.FindIndex(v => v.ToString() == "ID");
            var valueIndex = headerRow.FindIndex(v => v.ToString() == language);

            if (valueIndex < 0) valueIndex = headerRow.FindIndex(v => v.ToString() != "ID");

            if (keyIndex < 0 || valueIndex < 0) throw new I18nException($"The language '{language}' doesn't exist.");

            return (keyIndex, valueIndex);
        }

        private I18nScope MapBody(IList<IList<object>> bodyRows, int keyIndex, int valueIndex)
        {
            var warnings = new List<string>();
            var rootScope = new I18nScope(warnings);

            foreach (var row in bodyRows.Where(r => r.Count > valueIndex))
            {
                var keyValueDto = new I18nKeyValueDto(row[keyIndex], row[valueIndex]);
                rootScope.SetInScope(keyValueDto.ScopeSegments, keyValueDto.KeySegment, keyValueDto.Value);
            }

            return rootScope;
        }
    }
}