using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Infrastructure.Core;
using GoogleSheetI18n.Infrastructure.Services.I18nLocalStore;
using NUnit.Framework;

namespace GoogleSheetI18n.UnitTests
{
    public class I18nLocalStoreTests
    {
        [Test]
        public void Constructor_WhenFolderPathIsNotDefined_DoesNotThrow()
        {
            // arrange & act & assert
            Assert.DoesNotThrow(() => new I18nLocalStore(""));
        }

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
            var i18nLocalStore = new I18nLocalStore(Path.GetRandomFileName());

            // act
            await i18nLocalStore.SaveSheet(expectedI18nSheet);

            // assert
            var storedSheet = await i18nLocalStore.GetSheet(expectedI18nSheet.SpreadsheetId, expectedI18nSheet.SheetName);

            Assert.AreEqual(expectedI18nSheet.SpreadsheetId, storedSheet.SpreadsheetId);
            Assert.AreEqual(expectedI18nSheet.SheetName, storedSheet.SheetName);
            Assert.AreEqual(expectedI18nSheet.ValueRange.Values, storedSheet.ValueRange.Values);
        }
    }
}