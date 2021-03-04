using System.Collections.Generic;

namespace GoogleSheetI18n.Api.Core
{
    public record I18nCacheMetadata(List<I18nChannel> ExistingChannels, I18nChannel CurrentChannel);
}