using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Infrastructure.Core;
using GoogleSheetI18n.Infrastructure.Validation;
using NUnit.Framework;

namespace GoogleSheetI18n.UnitTests
{
    public class I18nCodeTranslationTests
    {
        private I18nCodeTranslation _i18nCodeTranslation;

        [SetUp]
        public void Setup()
        {
            _i18nCodeTranslation = new I18nCodeTranslation();
        }

        [Test]
        public void Validate_WhenSheetIsInvalid_ShouldReturnErrorMessage()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en", "ua"},
                    new List<object> {"btn.Word", "Слово", "word"},
                    new List<object> {"btn.Work", "Работа", "work"},
                    new List<object> {"btn.Word", "Слово", "word"},
                }
            };

            var i18nSheets = new List<I18nSheet>
            {
                new I18nSheet("spreadsheetId1", "sheetName1", valueRange)
            };

            // act
            var expectedValidationError = _i18nCodeTranslation.Validate(i18nSheets);

            Assert.AreEqual(expectedValidationError.Message, "The i18n for key '.btn.Word' already exists and will be overriden.");
        }

        [Test]
        public void Validate_WhenSheetIsValid_ErrorMessageShouldBeIsEmpty()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en", "ua"},
                    new List<object> {"btn.Word", "Слово", "word"},
                    new List<object> {"btn.Work", "Работа", "work"},
                }
            };

            var i18nSheets = new List<I18nSheet>
            {
                new I18nSheet("spreadsheetId1", "sheetName1", valueRange)
            };

            // act
            var expectedValidationError = _i18nCodeTranslation.Validate(i18nSheets);

            Assert.AreEqual(expectedValidationError.Message, string.Empty);
        }
    }
}
