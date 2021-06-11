using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheetI18n.Api.Attributes;
using GoogleSheetI18n.Common.Exceptions;
using GoogleSheetI18n.Common.Extensions;
using GoogleSheetI18n.Infrastructure.Features.ObjectTreeForLanguage;
using GoogleSheetI18n.Infrastructure.Models.I18n;
using GoogleSheetI18n.Infrastructure.Services.I18nGoogleClient;
using GoogleSheetI18n.Infrastructure.Services.I18nLocalStore;
using GoogleSheetI18n.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoogleSheetI18n.Api.Controllers
{
    [ApiController]
    [Route("i18n")]
    public class I18nController : ControllerBase
    {
        private readonly II18nLocalStore _i18nLocalStore;
        private readonly II18nGoogleClient _i18nGoogleClient;
        private readonly I18nOptions _i18nOptions;
        private readonly ILogger<I18nController> _logger;
        private readonly II18nCodeTranslation _i18nCodeTranslation;

        public I18nController(
            ILogger<I18nController> logger,
            II18nGoogleClient i18nGoogleClient,
            I18nOptions i18nOptions,
            II18nLocalStore i18nLocalStore,
            II18nCodeTranslation i18nCodeTranslation)
        {
            _logger = logger;
            _i18nGoogleClient = i18nGoogleClient;
            _i18nOptions = i18nOptions;
            _i18nLocalStore = i18nLocalStore;
            _i18nCodeTranslation = i18nCodeTranslation;
        }

        [HttpPost("reload-local-store")]
        [ClaimRequirement("i18n-admin", "")]
        public async Task ReloadLocalStore()
        {
            var newSheets = await _i18nGoogleClient.GetSheets(_i18nOptions.SpreadsheetId);

            await _i18nLocalStore.SaveSheets(newSheets);
        }

        [HttpGet("validate-spreadsheet")]
        public async Task<object> ValidateSpreadsheet()
        {
            var i18nSheets = await _i18nGoogleClient.GetSheets(_i18nOptions.SpreadsheetId);

            return _i18nCodeTranslation.Validate(i18nSheets);
        }

        [HttpGet("validate-local-spreadsheet")]
        public async Task<object> ValidateLocalSpreadsheet()
        {
            var i18nSheets = await _i18nLocalStore.GetSheets(_i18nOptions.SpreadsheetId);

            return _i18nCodeTranslation.Validate(i18nSheets);
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