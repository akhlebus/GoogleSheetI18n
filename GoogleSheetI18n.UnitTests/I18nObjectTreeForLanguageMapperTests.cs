using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Infrastructure.Features.ObjectTreeForLanguage;
using NUnit.Framework;

namespace GoogleSheetI18n.UnitTests
{
    public class I18nObjectTreeForLanguageMapperTests
    {
        [Test]
        public void Map_WhenDuplicateKeysAndLastKeyIsComplex_ReturnsWarningAndLastValue()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en"},
                    new List<object> {"Word", "Слово", "word"},
                    new List<object> {"Work", "Работа", "work"},
                    new List<object> {"Work.table", "Стол", "table"}
                }
            };

            // act
            var objectTree = new I18nObjectTreeForLanguageMapper().Map(valueRange, "en");

            // assert
            CollectionAssert.IsNotEmpty(objectTree.Warnings);
            Assert.AreEqual("table", objectTree.GetOrCreateScope("Work")["table"].ToString());
        }

        [Test]
        public void Map_WhenDuplicateKeysAndFirstKeyIsComplex_ReturnsWarningAndLastValue()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en"},
                    new List<object> {"Word", "Слово", "word"},
                    new List<object> {"Work.table", "Стол", "table"},
                    new List<object> {"Work", "Работа", "work"}
                }
            };

            // act
            var objectTree = new I18nObjectTreeForLanguageMapper().Map(valueRange, "en");

            // assert
            CollectionAssert.IsNotEmpty(objectTree.Warnings);
            Assert.AreEqual("work", objectTree["Work"].ToString());
        }

        [Test]
        public void Map_WhenDuplicateKeys_ReturnsWarningAndLastValue()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en"},
                    new List<object> {"Word", "Слово", "word"},
                    new List<object> {"Work", "Работа", "work"},
                    new List<object> {"Work", "Урок", "lesson"}
                }
            };

            // act
            var objectTree = new I18nObjectTreeForLanguageMapper().Map(valueRange, "en");

            // assert
            CollectionAssert.IsNotEmpty(objectTree.Warnings);
            Assert.AreEqual("lesson", objectTree["Work"].ToString());
        }

        [Test]
        public void Map_WhenLangIsEn_ReturnsEnglishData()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en"},
                    new List<object> {"Word", "Слово", "word"},
                    new List<object> {"Work", "Работа", "work"}
                }
            };

            // act
            var objectTree = new I18nObjectTreeForLanguageMapper().Map(valueRange, "en");

            // assert
            Assert.AreEqual("word", objectTree["Word"].ToString());
        }

        [Test]
        public void Map_WhenLangIsRuAndNestedProps_ReturnsRussianData()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en"},
                    new List<object> {"btn.Word", "Слово", "word"},
                    new List<object> {"btn.Work", "Работа", "work"},
                    new List<object> {"Work", "Работа", "work"}
                }
            };

            // act
            var objectTree = new I18nObjectTreeForLanguageMapper().Map(valueRange, "ru");

            // assert
            Assert.AreEqual("Слово", objectTree.GetOrCreateScope("btn")["Word"].ToString());
            Assert.AreEqual("Работа", objectTree.GetOrCreateScope("btn")["Work"].ToString());
            Assert.AreEqual("Работа", objectTree["Work"].ToString());
        }

        [Test]
        public void Map_WhenLangIsNotSuported_ReturnsDataInFirstNotIDColumn()
        {
            // arrange
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> {"ID", "ru", "en"},
                    new List<object> {"btn.Word", "Слово", "word"},
                    new List<object> {"btn.Work", "Работа", "work"},
                    new List<object> {"Work", "Работа", "work"}
                }
            };

            // act
            var objectTree = new I18nObjectTreeForLanguageMapper().Map(valueRange, "fr");

            // assert
            Assert.AreEqual("Слово", objectTree.GetOrCreateScope("btn")["Word"].ToString());
            Assert.AreEqual("Работа", objectTree.GetOrCreateScope("btn")["Work"].ToString());
            Assert.AreEqual("Работа", objectTree["Work"].ToString());
        }
    }
}