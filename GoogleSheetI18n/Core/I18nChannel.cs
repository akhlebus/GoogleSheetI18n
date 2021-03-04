using System;

namespace GoogleSheetI18n.Api.Core
{
    public record I18nChannel(string ChannelId, string ResourceId, DateTimeOffset? Expiration);
}