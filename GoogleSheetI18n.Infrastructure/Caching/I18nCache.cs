using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoogleSheetI18n.Infrastructure.Core;

namespace GoogleSheetI18n.Infrastructure.Caching
{
    public class I18nCache
    {
        private readonly Dictionary<string, I18nSheet> _cache = new();
        private readonly Semaphore _semaphore = new(1, 1);

        public I18nSheet this[string key]
        {
            get => _cache[key];
            set => _cache[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return _cache.ContainsKey(key);
        }

        public async Task<I18nSheet> GetOrAdd(string key, Func<string, Task<I18nSheet>> addItem)
        {
            if (ContainsKey(key)) return this[key];

            _semaphore.WaitOne();
            if (ContainsKey(key))
            {
                _semaphore.Release();
                return this[key];
            }

            var i18nSheet = await addItem(key);
            this[key] = i18nSheet;
            _semaphore.Release();

            return i18nSheet;
        }

        public void Set(string key, I18nSheet item)
        {
            _semaphore.WaitOne();
            this[key] = item;
            _semaphore.Release();
        }

        public IList<I18nSheet> Clear()
        {
            var removedValues = _cache.Values.ToArray();
            _cache.Clear();

            return removedValues;
        }
    }
}