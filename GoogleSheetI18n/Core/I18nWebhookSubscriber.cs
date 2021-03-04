using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using GoogleSheetI18n.Api.Exceptions;

namespace GoogleSheetI18n.Api.Core
{
    public class I18nWebhookSubscriber
    {
        private readonly I18nCache _i18nCache;
        private readonly I18nCredential _i18nCredential;

        public I18nWebhookSubscriber(string credentialFilePath, I18nCache i18nCache)
        {
            _i18nCredential = new I18nCredential(credentialFilePath);
            _i18nCache = i18nCache;
        }

        public async Task Subscribe(string subscriptionUrl, string spreedsheetId)
        {
            var driveService = await GetDriveService();

            var channel = new Channel
            {
                Id = Guid.NewGuid().ToString(),
                Type = "web_hook",
                Address = subscriptionUrl
            };

            var request = driveService.Files.Watch(channel, spreedsheetId);

            try
            {
                channel = await request.ExecuteAsync();
                var expiration = DateTimeOffset.FromUnixTimeMilliseconds(channel.Expiration.GetValueOrDefault());
                _i18nCache.AddChannel(channel.Id, channel.ResourceId, expiration, true);
            }
            catch (GoogleApiException exc)
            {
                throw new I18nException("Unable to subscribe to I18N google sheet updates.", exc);
            }
        }

        public async Task UnsubscribeFromExistingChannels()
        {
            var driveService = await GetDriveService();
            var metadata = _i18nCache.GetMetadata();

            foreach (var i18nChannel in metadata.ExistingChannels)
            {
                var deletingChannel = new Channel {Id = i18nChannel.ChannelId, ResourceId = i18nChannel.ResourceId};
                var channelsRequest = driveService.Channels.Stop(deletingChannel);

                try
                {
                    await channelsRequest.ExecuteAsync();
                }
                catch (GoogleApiException e)
                {
                }
            }

            metadata = metadata with { ExistingChannels = new List<I18nChannel>() };
            _i18nCache.SaveMetadata(metadata);
        }

        private async Task<DriveService> GetDriveService()
        {
            var credential = await _i18nCredential.GetValue();

            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleSheets"
            });
            return service;
        }
    }
}