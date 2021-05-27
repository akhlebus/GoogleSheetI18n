using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GoogleSheetI18n.Infrastructure.Caching;
using GoogleSheetI18n.Infrastructure.Models.I18n;
using GoogleSheetI18n.Common.Extensions;
using GoogleSheetI18n.Infrastructure.Services.I18nGoogleClient;
using GoogleSheetI18n.Infrastructure.Services.I18nLocalStore;
using GoogleSheetI18n.Infrastructure.Validation;

namespace GoogleSheetI18n.Api.IoC
{
    public static class GoogleSheetI18nRegistrator
    {
        public static void AddGoogleSheetI18n(this IServiceCollection services, IConfiguration configuration)
        {
            var i18nOptions = AddOptions(services, configuration);

            var i18nLocalStore = new I18nLocalStore(i18nOptions.LocalStorePath);
            services.AddSingleton(i18nLocalStore);

            var i18nCache = new I18nCache();
            services.AddSingleton(i18nCache);

            services.AddSingleton<II18nLocalStore>(new I18nCachableLocalStore(i18nLocalStore, i18nCache));

            var i18nGoogleClient = new I18nGoogleClient(i18nOptions.CredentialsFilePath);
            services.AddSingleton<II18nGoogleClient>(i18nGoogleClient);

            services.AddSingleton<II18nCodeTranslation>(new I18nCodeTranslation());
        }

        private static I18nOptions AddOptions(IServiceCollection services, IConfiguration configuration)
        {
            var i18nOptionsSection = configuration.GetSection(nameof(I18nOptions));
            var i18nOptions = i18nOptionsSection.Get<I18nOptions>();
            services.AddSingleton(i18nOptions);

            i18nOptions.SpreadsheetId = GetEnvVariable(nameof(I18nOptions.SpreadsheetId)) ?? i18nOptions.SpreadsheetId;
            i18nOptions.LocalStorePath = GetEnvVariable(nameof(I18nOptions.LocalStorePath)) ?? i18nOptions.LocalStorePath;
            i18nOptions.CredentialsFilePath = GetEnvVariable(nameof(I18nOptions.CredentialsFilePath)) ?? i18nOptions.CredentialsFilePath;
            i18nOptions.SubscriptionUrl = GetEnvVariable(nameof(I18nOptions.SubscriptionUrl)) ?? i18nOptions.SubscriptionUrl;

            return i18nOptions;
        }

        private static string GetEnvVariable(string name) => Environment.GetEnvironmentVariable(name.FromPascalCaseToUnderscoreUpperCase());
    }
}
