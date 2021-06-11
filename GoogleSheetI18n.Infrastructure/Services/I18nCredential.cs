using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;

namespace GoogleSheetI18n.Infrastructure.Services
{
    internal class I18nCredential
    {
        public const string GoogleApplicationCredentialsAsJson = "GOOGLE_APPLICATION_CREDENTIALS_AS_JSON";

        private readonly Lazy<Task<ICredential>> _cachedCredentialTask;

        public I18nCredential(string credentialFilePath = null)
        {
            credentialFilePath = !string.IsNullOrEmpty(credentialFilePath) && !Path.IsPathRooted(credentialFilePath)
                ? $@"{AppDomain.CurrentDomain.BaseDirectory}\{credentialFilePath}"
                : credentialFilePath;

            async Task<ICredential> CreateCredentials()
            {
                return (await GetGoogleSheetCredentials(credentialFilePath))
                    .CreateScoped(SheetsService.Scope.Drive, SheetsService.Scope.Spreadsheets);
            }

            _cachedCredentialTask = new Lazy<Task<ICredential>>(CreateCredentials);
        }

        public Task<ICredential> GetValue() => _cachedCredentialTask.Value;

        private async Task<GoogleCredential> GetGoogleSheetCredentials(string credentialFilePath)
        {
            var credentialAsJson = Environment.GetEnvironmentVariable(GoogleApplicationCredentialsAsJson);
            if (credentialAsJson != null)
                return GoogleCredential.FromJson(credentialAsJson);

            return credentialFilePath switch
            {
                null or "" => await GoogleCredential.GetApplicationDefaultAsync(),
                _ => await GoogleCredential.FromFileAsync(credentialFilePath, CancellationToken.None)
            };
        }
    }
}