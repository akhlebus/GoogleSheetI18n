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
        private readonly I18nBackup _i18nBackup;
        private readonly I18nCache _i18nCache;
        private readonly II18nClient _i18nClient;
        private readonly I18nOptions _i18nOptions;
        private readonly ILogger<I18nController> _logger;

        public I18nController(
            ILogger<I18nController> logger,
            II18nClient i18NClient,
            I18nOptions i18nOptions,
            I18nCache i18nCache,
            I18nBackup i18nBackup)
        {
            _logger = logger;
            _i18nClient = i18NClient;
            _i18nOptions = i18nOptions;
            _i18nCache = i18nCache;
            _i18nBackup = i18nBackup;
        }

        [HttpPost("empty-cache")]
        public async Task ClearCache()
        {
            var channelId = Request.Headers["X-Goog-Channel-ID"].ToString();
            var resourceId = Request.Headers["X-Goog-Resource-ID"].ToString();

            if (channelId is null or "" || resourceId is null or "") return;

            _logger.LogInformation($"{nameof(ClearCache)} (Channel ID: {channelId}, Resource ID: {resourceId})");

            if (_i18nCache.IsCurrentChannel(channelId, resourceId))
            {
                var deletedI18nSheets = _i18nCache.Clear();

                foreach (var i18nSheet in deletedI18nSheets)
                {
                    await _i18nBackup.SaveSheet(i18nSheet);
                }
            }
            else
            {
                _i18nCache.AddChannel(channelId, resourceId, null, false);
            }
        }

        [HttpGet("languages")]
        public async Task<IReadOnlyList<string>> GetSupportedLanguages()
        {
            try
            {
                var i18nSheet = await _i18nClient.GetSheet(_i18nOptions.SpreadsheetId, "");
                return i18nSheet.GetSupportedLanguages();
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
                var i18nSheet = await _i18nClient.GetSheet(_i18nOptions.SpreadsheetId, sheetName);
                return i18nSheet.GetTranslations(lng);
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
            var metadata = _i18nCache.GetMetadata();

            return new
            {
                _i18nOptions.SpreadsheetId,
                _i18nOptions.SubscriptionUrl,
                metadata.CurrentChannel
            };
        }
    }
}