using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetI18n.Api.Tests.Core;
using GoogleSheetI18n.Infrastructure.Core;
using GoogleSheetI18n.Infrastructure.Services.I18nGoogleClient;
using GoogleSheetI18n.Infrastructure.Services.I18nLocalStore;
using NUnit.Framework;

namespace GoogleSheetI18n.Api.Tests.Integration
{
    public class I18nLocalStoreTests
    {
        private readonly string _spreadsheetId = TestContextParams.SpreadSheetId;
        private IList<I18nSheet> _sheets;

        [OneTimeSetUp]
        public async Task Setup()
        {
            var i18nClient = new I18nGoogleClient();
            _sheets = await i18nClient.GetSheets(_spreadsheetId);
        }

        [Test]
        public async Task GetSheet_WhenLangIsEn_ReturnsEnglishData()
        {
            // arrange
            var i18nLocalStore = new I18nLocalStore(System.IO.Path.GetRandomFileName());
            await i18nLocalStore.SaveSheets(_sheets);

            // act
            var i18nSheet = await i18nLocalStore.GetSheet(_spreadsheetId, "Global");

            // assert
            var objectTree = i18nSheet.GetTranslations("en");

            Assert.AreEqual("Home", objectTree.GetOrCreateScope("navbar")["home"].ToString());
        }

        [Test]
        public async Task GetSheet_WhenSheetNameIsEmptyAndLangIsEn_ReturnsDataOfFirstSheet()
        {
            // arrange
            var i18nLocalStore = new I18nLocalStore(System.IO.Path.GetRandomFileName());
            await i18nLocalStore.SaveSheets(_sheets);

            // act & assert
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await i18nLocalStore.GetSheet(_spreadsheetId, "")
            );
        }

        [Test]
        public async Task GetSheet_WhenSheetNameIsUnknown_ReturnsNull()
        {
            // arrange
            var i18nLocalStore = new I18nLocalStore(System.IO.Path.GetRandomFileName());
            await i18nLocalStore.SaveSheets(_sheets);

            // act
            var sheet = await i18nLocalStore.GetSheet(_spreadsheetId, "NonExistingSheetName");

            // assert
            Assert.IsNull(sheet);
        }
    }
}