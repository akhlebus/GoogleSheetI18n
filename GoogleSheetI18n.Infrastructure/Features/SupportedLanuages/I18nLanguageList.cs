using System.Collections.Generic;

namespace GoogleSheetI18n.Infrastructure.Features.SupportedLanuages
{
    public class I18nLanguageList : List<string>
    {
        public I18nLanguageList(IEnumerable<string> languages, List<string> warnings) : base(languages)
        {
            Warnings = warnings;
        }

        public List<string> Warnings { get; init; }
    }
}