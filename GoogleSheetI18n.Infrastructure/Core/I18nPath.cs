using System;
using System.Linq;

namespace GoogleSheetI18n.Infrastructure.Core
{
    public static class I18nPath
    {
        public static char Separator { get; } = '.';

        public static string Combine(string path, string segment)
        {
            return $"{path}{Separator}{segment}";
        }

        public static (string[] AllSegments, string[] ScopeSegments, string KeySegment) GetPathSegments(string path)
        {
            var keySegments = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
            var keySegmentsWithoutLast = keySegments.Take(keySegments.Length - 1).ToArray();
            var lastKeySegment = keySegments.Last();

            return (keySegments, keySegmentsWithoutLast, lastKeySegment);
        }
    }
}
