using System;
using GoogleSheetI18n.Api.Core;
using GoogleSheetI18n.Api.SimpleWebApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GoogleSheetI18n.Api.Integrations.Web
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGoogleSheetI18n(this IServiceCollection services, IConfiguration configuration)
        {
            var i18nOptions = AddOptions(services, configuration, out var i18nBackup);

            var i18nClient = new I18nClient(i18nOptions.CredentialsFilePath);
            var i18nCache = new I18nCache(i18nOptions.BackupFolderPath);
            services.AddSingleton(i18nCache);

            var i18nCachableClient = new I18nCachableClient(i18nClient, i18nCache);
            var i18nBackupableClient = new I18nBackupableClient(i18nCachableClient, i18nBackup);
            services.AddSingleton<II18nClient>(i18nBackupableClient);

            var i18nWebhookSubscriber = new I18nWebhookSubscriber(i18nOptions.CredentialsFilePath, i18nCache);
            i18nWebhookSubscriber.UnsubscribeFromExistingChannels().Wait();
            services.AddSingleton(i18nWebhookSubscriber);

            if (i18nOptions.SubscriptionUrl is not null and not "")
            {
                i18nWebhookSubscriber.Subscribe(i18nOptions.SubscriptionUrl, i18nOptions.SpreadsheetId).Wait();
            }
        }

        private static I18nOptions AddOptions(IServiceCollection services, IConfiguration configuration, out I18nBackup i18nBackup)
        {
            var i18nOptionsSection = configuration.GetSection(nameof(I18nOptions));
            var i18nOptions = i18nOptionsSection.Get<I18nOptions>();
            services.AddSingleton(i18nOptions);

            i18nOptions.SpreadsheetId = GetEnvVariable(nameof(I18nOptions.SpreadsheetId)) ?? i18nOptions.SpreadsheetId;
            i18nOptions.BackupFolderPath = GetEnvVariable(nameof(I18nOptions.BackupFolderPath)) ?? i18nOptions.BackupFolderPath;
            i18nOptions.CredentialsFilePath = GetEnvVariable(nameof(I18nOptions.CredentialsFilePath)) ?? i18nOptions.CredentialsFilePath;
            i18nOptions.SubscriptionUrl = GetEnvVariable(nameof(I18nOptions.SubscriptionUrl)) ?? i18nOptions.SubscriptionUrl;

            i18nBackup = new I18nBackup(i18nOptions.BackupFolderPath);
            services.AddSingleton(i18nBackup);
            return i18nOptions;
        }

        private static string GetEnvVariable(string name) => Environment.GetEnvironmentVariable(name.FromPascalCaseToUnderscoreUpperCase());
    }
}
