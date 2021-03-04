using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Api.Core;
using NUnit.Framework;

namespace GoogleSheetI18n.Api.Tests.Unit
{
    public class I18nCacheTests
    {
        [Test]
        public void SaveMetadata_WhenDataIsValid_Saves()
        {
            // arrange
            var expectedMetadata = new I18nCacheMetadata(
                new List<I18nChannel>
                {
                    new ("Ch1", "R1", DateTimeOffset.MaxValue),
                    new ("Ch2", "R2", DateTimeOffset.MinValue),
                    new ("Ch3", "R3", null)
                }, new ("Ch1", "R1", DateTimeOffset.MaxValue)
            );
            var i18nCache = new I18nCache(Path.GetRandomFileName());

            // act
            i18nCache.SaveMetadata(expectedMetadata);

            // assert
            var storedMetadata = i18nCache.GetMetadata();

            Assert.AreEqual(expectedMetadata.CurrentChannel, storedMetadata.CurrentChannel);
            CollectionAssert.AreEqual(expectedMetadata.ExistingChannels, storedMetadata.ExistingChannels);
        }

        [Test]
        public async Task GetOrAdd_WhenExecutesConcurrently_ReturnsTheSameObject()
        {
            // arrange
            var expectedMetadata = new I18nCacheMetadata(
                new List<I18nChannel>
                {
                    new ("Ch1", "R1", DateTimeOffset.MaxValue),
                    new ("Ch2", "R2", DateTimeOffset.MinValue),
                    new ("Ch3", "R3", null)
                }, new("Ch1", "R1", DateTimeOffset.MaxValue)
            );
            var i18nCache = new I18nCache(Path.GetRandomFileName());

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
            var i18nCache = new I18nCache(Path.GetRandomFileName());
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

        [Test]
        public void AddChannel_WhenChannelIsCurrent_SetsCurrentAndChannelIsInExisting()
        {
            // arrange
            var i18nCache = new I18nCache(Path.GetRandomFileName());

            // act
            i18nCache.AddChannel("ch1", "r1", DateTimeOffset.MaxValue, true);

            // assert
            var metadata = i18nCache.GetMetadata();
            Assert.AreEqual("ch1", metadata.CurrentChannel.ChannelId);
            Assert.AreEqual(1, metadata.ExistingChannels.Count);
            Assert.AreEqual("ch1", metadata.ExistingChannels.First().ChannelId);
        }

        [Test]
        public void AddChannel_WhenChannelIsNotCurrent_CurrentIsNullAndChannelIsInExisting()
        {
            // arrange
            var i18nCache = new I18nCache(Path.GetRandomFileName());

            // act
            i18nCache.AddChannel("ch1", "r1", DateTimeOffset.MaxValue, false);

            // assert
            var metadata = i18nCache.GetMetadata();
            Assert.IsNull(metadata.CurrentChannel);
            Assert.AreEqual(1, metadata.ExistingChannels.Count);
            Assert.AreEqual("ch1", metadata.ExistingChannels.First().ChannelId);
        }

        [Test]
        public void IsCurrentChannel_WhenChannelIsCurrent_ReturnsTrue()
        {
            // arrange
            var i18nCache = new I18nCache(Path.GetRandomFileName());
            i18nCache.AddChannel("ch1", "r1", DateTimeOffset.MaxValue, false);
            i18nCache.AddChannel("ch2", "r2", DateTimeOffset.MaxValue, true);
            i18nCache.AddChannel("ch3", "r3", DateTimeOffset.MaxValue, false);

            // act & assert
            Assert.IsTrue(i18nCache.IsCurrentChannel("ch2", "r2"));
        }

        [Test]
        public void IsCurrentChannel_WhenChannelIsCurrent_ReturnsFalse()
        {
            // arrange
            var i18nCache = new I18nCache(Path.GetRandomFileName());
            i18nCache.AddChannel("ch1", "r1", DateTimeOffset.MaxValue, false);
            i18nCache.AddChannel("ch2", "r2", DateTimeOffset.MaxValue, true);
            i18nCache.AddChannel("ch3", "r3", DateTimeOffset.MaxValue, false);

            // act & assert
            Assert.IsFalse(i18nCache.IsCurrentChannel("ch3", "r3"));
        }

        [Test]
        public void IsCurrentChannel_WhenChannelIsNotInExisting_ReturnsFalse()
        {
            // arrange
            var i18nCache = new I18nCache(Path.GetRandomFileName());
            i18nCache.AddChannel("ch1", "r1", DateTimeOffset.MaxValue, false);
            i18nCache.AddChannel("ch2", "r2", DateTimeOffset.MaxValue, true);
            i18nCache.AddChannel("ch3", "r3", DateTimeOffset.MaxValue, false);

            // act & assert
            Assert.IsFalse(i18nCache.IsCurrentChannel("ch4", "r3"));
        }
    }
}
