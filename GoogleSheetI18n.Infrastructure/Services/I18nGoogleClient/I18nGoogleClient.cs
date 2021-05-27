using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetI18n.Common.Exceptions;
using GoogleSheetI18n.Infrastructure.Core;

namespace GoogleSheetI18n.Infrastructure.Services.I18nGoogleClient
{
    public class I18nGoogleClient : II18nGoogleClient
    {
        private readonly I18nCredential _i18nCredential;

        public I18nGoogleClient(string credentialFilePath = null)
        {
            _i18nCredential = new I18nCredential(credentialFilePath);
        }

        public async Task<IList<I18nSheet>> GetSheets(string spreadsheetId)
        {
            var credential = await _i18nCredential.GetValue();
            var sheets = await GetGoogleSheetValues(credential, spreadsheetId);

            return sheets
                .Select(s => new I18nSheet(spreadsheetId, s.SheetName, s.Values))
                .ToArray();
        }

        private async Task<IList<(string SheetName, ValueRange Values)>> GetGoogleSheetValues(
            ICredential credential, string spreadsheetId)
        {
            var service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleSheets"
            });

            try
            {
                var request = service.Spreadsheets.Get(spreadsheetId);
                var spreadsheet = await request.ExecuteAsync();

                var tasks = spreadsheet.Sheets.Select(
                    s => GetGoogleSheetValues(service, spreadsheetId, s)
                );
                return await Task.WhenAll(tasks);
            }
            catch (GoogleApiException exc)
            {
                throw new I18nException("Unable to retrieve google sheet values.", exc);
            }
        }

        private static async Task<(string SheetName, ValueRange Values)> GetGoogleSheetValues(
            SheetsService service, string spreadsheetId, Sheet sheetDto)
        {
            var range = $"{sheetDto.Properties.Title}!A:Z";
            var sheetRequest = service.Spreadsheets.Values.Get(spreadsheetId, range);
            var sheet = await sheetRequest.ExecuteAsync();
            return new(sheetDto.Properties.Title, sheet);
        }
    }
}