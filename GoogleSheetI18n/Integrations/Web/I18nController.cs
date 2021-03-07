using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetI18n.Api.Core;
using GoogleSheetI18n.Api.Entities;
using GoogleSheetI18n.Api.Exceptions;
using GoogleSheetI18n.Api.SimpleWebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoogleSheetI18n.Api.Integrations.Web
{
    [ApiController]
    [Route("[controller]")]
    public class I18nController : ControllerBase
    {
        private readonly II18nLocalStore _i18nLocalStore;
        private readonly I18nCache _i18nCache;
        private readonly II18nGoogleClient _i18nGoogleClient;
        private readonly I18nOptions _i18nOptions;
        private readonly ILogger<I18nController> _logger;

        public I18nController(
            ILogger<I18nController> logger,
            II18nGoogleClient i18nGoogleClient,
            I18nOptions i18nOptions,
            I18nCache i18nCache,
            II18nLocalStore i18nLocalStore)
        {
            _logger = logger;
            _i18nGoogleClient = i18nGoogleClient;
            _i18nOptions = i18nOptions;
            _i18nCache = i18nCache;
            _i18nLocalStore = i18nLocalStore;
        }

        [HttpPost("reload-local-store")]
        [ClaimRequirement("i18n-admin", "")]
        public async Task ReloadLocalStore()
        {
            var newSheets = await _i18nGoogleClient.GetSheets(_i18nOptions.SpreadsheetId);

            await _i18nLocalStore.SaveSheets(newSheets);
        }

        [HttpGet("languages")]
        public async Task<IReadOnlyList<string>> GetSupportedLanguages()
        {
            try
            {
                var i18nSheet = await _i18nLocalStore.GetSheet(_i18nOptions.SpreadsheetId, "Global");
                return i18nSheet == null ? Array.Empty<string>() : i18nSheet.GetSupportedLanguages();
            }
            catch (I18nException e)
            {
                _logger.LogError(e, "Bad sheet format.");
                return Array.Empty<string>();
            }
        }

        [HttpGet("{lng}/{ns}")]
        public async Task<Dictionary<string, object>> Get(string lng, string ns)
        {
            var sheetName = ns.FromDashCaseToPascalCase();

            try
            {
                var i18nSheet = await _i18nLocalStore.GetSheet(_i18nOptions.SpreadsheetId, sheetName);
                return i18nSheet == null ?
                    new I18nScope(new List<string>()) :
                    i18nSheet.GetTranslations(lng);
            }
            catch (I18nException e)
            {
                _logger.LogError(e, "Bad sheet format.");
                return new I18nScope(new List<string>());
            }
        }

        [HttpGet("settings")]
        public object Get()
        {
            return new
            {
                _i18nOptions.SpreadsheetId,
                _i18nOptions.SubscriptionUrl
            };
        }
    }
}