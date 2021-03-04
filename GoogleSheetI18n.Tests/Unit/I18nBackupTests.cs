using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Api.Core;
using GoogleSheetI18n.Api.Exceptions;
using GoogleSheetI18n.Api.Features.SupportedLanuages;
using NUnit.Framework;

namespace GoogleSheetI18n.Api.Tests.Unit
{
    public class I18nBackupTests
    {
        [Test]
        public async Task SaveSheet_WhenDataIsValid_Saves()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en", "ua"},
                    new List<object> {"btn.Word", "Слово", "word"},
                    new List<object> {"btn.Work", "Работа", "work"},
                    new List<object> {"Work", "Работа", "work"}
                }
            };

            var expectedI18nSheet = new I18nSheet("spreadsheetId1", "sheetName1", valueRange);
            var i18nBackup = new I18nBackup(Path.GetRandomFileName());

            // act
            await i18nBackup.SaveSheet(expectedI18nSheet);

            // assert
            var storedSheet = await i18nBackup.GetSheet(expectedI18nSheet.SpreadsheetId, expectedI18nSheet.SheetName);

            Assert.AreEqual(expectedI18nSheet.SpreadsheetId, storedSheet.SpreadsheetId);
            Assert.AreEqual(expectedI18nSheet.SheetName, storedSheet.SheetName);
            Assert.AreEqual(expectedI18nSheet.ValueRange.Values, storedSheet.ValueRange.Values);
        }
    }
}