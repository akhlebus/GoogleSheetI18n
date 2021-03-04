using System.Threading.Tasks;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Api.Exceptions;

namespace GoogleSheetI18n.Api.Core
{
    public class I18nClient : II18nClient
    {
        private readonly I18nCredential _i18nCredential;

        public I18nClient(string credentialFilePath = null)
        {
            _i18nCredential = new I18nCredential(credentialFilePath);
        }

        public async Task<I18nSheet> GetSheet(string spreadsheetId, string sheetName)
        {
            var credential = await _i18nCredential.GetValue();
            var response = await GetGoogleSheetValues(credential, spreadsheetId, sheetName);

            return new I18nSheet(spreadsheetId, sheetName, response);
        }

        private async Task<ValueRange> GetGoogleSheetValues(ICredential credential, string spreadsheetId,
            string sheetName = "")
        {
            var service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleSheets"
            });

            var range = $"{sheetName}!A:Z";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            try
            {
                var response = await request.ExecuteAsync();

                return response;
            }
            catch (GoogleApiException exc)
            {
                throw new I18nException("Unable to retrieve google sheet values.", exc);
            }
        }
    }
}