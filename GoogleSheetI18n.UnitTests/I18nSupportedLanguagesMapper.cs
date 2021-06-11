using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Common.Exceptions;
using GoogleSheetI18n.Infrastructure.Features.SupportedLanuages;
using NUnit.Framework;

namespace GoogleSheetI18n.UnitTests
{
    public class I18nSupportedLanguagesMapperTests
    {
        [Test]
        public void Map_WhenLangListIsDefined_ReturnsLanguages()
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

            // act
            var languages = new I18nSupportedLanguagesMapper().Map(valueRange);

            // assert
            CollectionAssert.AreEqual(new[] { "ru", "en", "ua" }, languages);
        }

        [Test]
        public void Map_WhenLangListIsEmpty_ReturnsEmptyList()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID"},
                    new List<object> {"btn.Word", "Слово", "word"},
                    new List<object> {"btn.Work", "Работа", "work"},
                    new List<object> {"Work", "Работа", "work"}
                }
            };

            // act
            var languages = new I18nSupportedLanguagesMapper().Map(valueRange);

            // assert
            CollectionAssert.IsEmpty(languages);
        }

        [Test]
        public void Map_WhenNoHeader_ThrowsI18nException()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>()
            };

            // act & assert
            Assert.Throws<I18nException>(() => new I18nSupportedLanguagesMapper().Map(valueRange));
        }
    }
}