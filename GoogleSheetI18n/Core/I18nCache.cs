using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleSheetI18n.Api.Core
{
    public class I18nCache
    {
        public readonly string _backupsFolderPath;
        private readonly Dictionary<string, I18nSheet> _cache = new();
        private readonly Semaphore _semaphore = new(1, 1);

        public I18nCache(string backupsFolderPath)
        {
            _backupsFolderPath = !string.IsNullOrEmpty(backupsFolderPath) && !Path.IsPathRooted(backupsFolderPath)
                ? $@"{Environment.CurrentDirectory}\{backupsFolderPath}"
                : backupsFolderPath;

            if (backupsFolderPath is not null and not "")
            {
                Directory.CreateDirectory(_backupsFolderPath);
            }
        }

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

        public bool IsCurrentChannel(string sourceChannelId, string sourceResourceId)
        {
            var (_, currentChannel) = GetMetadata();

            return currentChannel.ChannelId == sourceChannelId &&
                   currentChannel.ResourceId == sourceResourceId;
        }

        public void AddChannel(string channelId, string resourceId, DateTimeOffset? expiration, bool isCurrent)
        {
            var metadata = GetMetadata();
            var channel = new I18nChannel(channelId, resourceId, expiration);

            if (!metadata.ExistingChannels.Any(ec => ec.ChannelId == channelId && ec.ResourceId == resourceId))
                metadata.ExistingChannels.Add(channel);

            if (isCurrent)
            {
                metadata = metadata with { CurrentChannel = channel };
            }

            SaveMetadata(metadata);
        }

        public IList<I18nSheet> Clear()
        {
            var removedValues = _cache.Values.ToArray();
            _cache.Clear();

            return removedValues;
        }

        public I18nCacheMetadata GetMetadata()
        {
            var metadataPath = Path.Combine(_backupsFolderPath, "cache-metadata");
            var metadataContent = File.Exists(metadataPath) ? File.ReadAllText(metadataPath) : string.Empty;
            var metadata = JsonConvert.DeserializeObject<I18nCacheMetadata>(metadataContent);
            return metadata ?? new I18nCacheMetadata(new List<I18nChannel>(), null);
        }

        public void SaveMetadata(I18nCacheMetadata metadata)
        {
            var metadataPath = Path.Combine(_backupsFolderPath, "cache-metadata");
            var metadataContent = JsonConvert.SerializeObject(metadata);
            File.WriteAllText(metadataPath, metadataContent);
        }
    }
}