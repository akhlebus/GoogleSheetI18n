using System.Collections.Generic;
using GoogleSheetI18n.Infrastructure.Core;

namespace GoogleSheetI18n.Infrastructure.Features.ObjectTreeForLanguage
{
    public class I18nScope : Dictionary<string, object>
    {
        public I18nScope(List<string> warnings, string path = "")
        {
            Warnings = warnings;
            Path = path;
        }

        public List<string> Warnings { get; init; }

        public string Path { get; init; }

        public I18nScope GetScope(string key)
        {
            return this.GetValueOrDefault(key) as I18nScope;
        }

        public void SetInScope(string[] scopeSegments, string key, string value)
        {
            var targetScope = GetOrCreateScopeByPath(scopeSegments);

            targetScope.SetInScope(key, value);
        }

        public I18nScope GetOrCreateScopeByPath(string[] scopeSegments)
        {
            var currentScope = this;

            foreach (var scopeSegment in scopeSegments) currentScope = currentScope.GetOrCreateScope(scopeSegment);

            return currentScope;
        }

        public I18nScope GetOrCreateScope(string key)
        {
            var existingValue = this.GetValueOrDefault(key);

            if (existingValue is not null and not I18nScope)
                Warnings.Add(
                    $"The i18n for key '{I18nPath.Combine(Path, key)}' already exists and will be overriden."
                );

            if (existingValue is not I18nScope) this[key] = new I18nScope(Warnings, I18nPath.Combine(Path, key));

            return (I18nScope)this[key];
        }

        public void SetInScope(string key, string value)
        {
            if (ContainsKey(key))
                Warnings.Add($"The i18n for key '{I18nPath.Combine(Path, key)}' already exists and will be overriden.");

            this[key] = value;
        }
    }
}