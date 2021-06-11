using System;
using System.Linq;

namespace GoogleSheetI18n.Common.Extensions
{
    public static class StringExtensions
    {
        public static string FromDashCaseToPascalCase(this string str)
        {
            var words = str
                .Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1));

            return string.Concat(words);
        }

        public static string FromPascalCaseToUnderscoreUpperCase(this string str)
        {
            return string.Concat(str.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())).ToUpperInvariant();
        }

        public static string FromPascalCaseToDashLowerCase(this string str)
        {
            return string.Concat(str.Select((c, i) => i > 0 && char.IsUpper(c) ? "-" + c : c.ToString())).ToLowerInvariant();
        }
    }
}