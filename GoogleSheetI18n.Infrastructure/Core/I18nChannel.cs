using System;

namespace GoogleSheetI18n.Infrastructure.Core
{
    public record I18nChannel(string ChannelId, string ResourceId, DateTimeOffset? Expiration);
}