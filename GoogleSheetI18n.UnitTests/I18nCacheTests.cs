using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Infrastructure.Caching;
using GoogleSheetI18n.Infrastructure.Core;
using NUnit.Framework;

namespace GoogleSheetI18n.UnitTests
{
    public class I18nCacheTests
    {
        [Test]
        public async Task GetOrAdd_WhenExecutesConcurrently_ReturnsTheSameObject()
        {
            // arrange
            var i18nCache = new I18nCache();

            // act
            var numberOfThreads = 1000;
            var tasks = new List<Task<I18nSheet>>(numberOfThreads);
            for (var i = 0; i < tasks.Capacity; ++i)
            {
                tasks.Add(i18nCache.GetOrAdd("temp", async key =>
                {
                    await Task.Delay(500);
                    return new I18nSheet("s1", "sh1", new ValueRange());
                }));
            }

            await Task.WhenAll(tasks);

            // assert
            var i18nSheets = tasks.Select(t => t.Result).ToList();
            var expectedI18nSheets = new List<I18nSheet>(numberOfThreads);
            for (var i = 0; i < tasks.Capacity; ++i)
            {
                expectedI18nSheets.Add(i18nSheets.First());
            }

            CollectionAssert.AreEquivalent(expectedI18nSheets, i18nSheets);
        }

        [Test]
        public async Task Clear_WhenHasData_ClearsData()
        {
            // arrange
            var i18nCache = new I18nCache();
            var item = await i18nCache.GetOrAdd(
                "ch1",
                key => Task.FromResult(new I18nSheet("1", "1", new ValueRange()))
            );

            // act
            i18nCache.Clear();

            // assert
            item = await i18nCache.GetOrAdd(
                "ch1",
                key => Task.FromResult(new I18nSheet("2", "2", new ValueRange()))
            );

            Assert.AreEqual("2", item.SpreadsheetId);
        }
    }
}
