using System.Threading.Tasks;
using GoogleSheetI18n.Api.Core;
using GoogleSheetI18n.Api.Exceptions;
using GoogleSheetI18n.Api.Tests.Core;
using NUnit.Framework;

namespace GoogleSheetI18n.Api.Tests.Integration
{
    public class I18nTests
    {
        private readonly string _spreadsheetId = TestContextParams.SpreadSheetId;

        [Test]
        public async Task GetSheet_WhenLangIsEn_ReturnsEnglishData()
        {
            // arrange
            var i18NClient = new I18nClient();

            // act
            var i18nSheet = await i18NClient.GetSheet(_spreadsheetId, "Global");

            // assert
            var objectTree = i18nSheet.GetTranslations("en");

            Assert.AreEqual("Home", objectTree.GetOrCreateScope("navbar")["home"].ToString());
        }

        [Test]
        public async Task GetSheet_WhenSheetNameIsEmptyAndLangIsEn_ReturnsDataOfFirstSheet()
        {
            // arrange
            var i18NClient = new I18nClient();

            // act
            var i18nSheet = await i18NClient.GetSheet(_spreadsheetId, "");

            // assert
            var objectTree = i18nSheet.GetTranslations("en");

            Assert.AreEqual("Home", objectTree.GetOrCreateScope("navbar")["home"].ToString());
        }

        [Test]
        public void GetSheet_WhenSheetNameIsNotValid_Throws()
        {
            // arrange
            var i18NClient = new I18nClient();

            // act & assert
            Assert.ThrowsAsync<I18nException>(async () =>
                await i18NClient.GetSheet(_spreadsheetId, "NonExistingSheetName"));
        }
    }
}