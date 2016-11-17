using System;
using StackExchange.Redis;
using SFA.Apprenticeships.Application.Interfaces.Caching;

namespace SFA.Apprenticeships.Infrastructure.Caching.Memory
{
    using System.Linq;
    using Application.Interfaces;
    using Newtonsoft.Json;
    using SFA.Infrastructure.Configuration;

    public class AzureRedisCacheService : ICacheService
    {
        private const string GettingItemFromCacheFormat = "Redis: Getting item with key: {0} from cache";
        private const string ItemReturnedFromCacheFormat = "Redis: Item with key: {0} returned from cache";
        private const string ItemNotInCacheFormat = "Redis: Item with key: {0} not in cache";

        private readonly object _locker = new object();
        private readonly ILogService _logger;
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        public AzureRedisCacheService(ILogService logService)
        {
            _logger = logService;

            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var cacheConnection = new ConfigurationManager().GetAppSetting("CacheConnection");
                return ConnectionMultiplexer.Connect(cacheConnection);
            });
        }

        private ConnectionMultiplexer Connection => _lazyConnection.Value;

        private IDatabase Cache => Connection.GetDatabase();

        public T Get<T>(string key) where T : class
        {
            _logger.Debug(GettingItemFromCacheFormat, key);

            var result = default(T);

            var value = Cache.StringGet(key);
            if (value.HasValue)
            {
                result = JsonConvert.DeserializeObject<T>(value);
                if (result == null || result.Equals(default(T)))
                {
                    _logger.Debug(ItemNotInCacheFormat, key);
                }
            }
            else
            {
                _logger.Debug(ItemNotInCacheFormat, key);
            }

            return result;
        }

        public TResult Get<TCacheEntry, TResult>(TCacheEntry cacheEntry, Func<TResult> dataFunc) where TCacheEntry : BaseCacheKey where TResult : class
        {
            var cacheKey = cacheEntry.Key();

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            //MemoryCache is thread safe however only the access is protected. The cache pattern of check then retrieve if null is not protected.
            //This allows multiple threads to execute the dataFunc uneccessarily. A lock here solves this issue
            lock (_locker)
            {
                var result = Get<TResult>(cacheKey);

                if (result == null || result.Equals(default(TResult)))
                {
                    result = dataFunc();

                    PutObject(cacheKey, result, cacheEntry.Duration);

                    return result;
                }

                _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

                return result;
            }
        }

        public TResult Get<TCacheEntry, TFuncParam1, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TResult> dataFunc, TFuncParam1 funcParam1) where TCacheEntry : BaseCacheKey where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1);

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            //MemoryCache is thread safe however only the access is protected. The cache pattern of check then retrieve if null is not protected.
            //This allows multiple threads to execute the dataFunc uneccessarily. A lock here solves this issue
            lock (_locker)
            {
                var result = Get<TResult>(cacheKey);

                if (result == null || result.Equals(default(TResult)))
                {
                    result = dataFunc(funcParam1);

                    PutObject(cacheKey, result, cacheEntry.Duration);

                    return result;
                }

                _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

                return result;
            }
        }

        public void PutObject(string cacheKey, object cacheObject, CacheDuration cacheDuration = CacheDuration.CacheDefault)
        {
            _logger.Debug("Storing item with key: {0} in cache with duration: {1}", cacheKey, cacheDuration);

            if (cacheObject == null)
            {
                return;
            }

            var cacheTimeSpan = TimeSpan.FromMinutes((int)cacheDuration);

            Cache.StringSet(cacheKey, JsonConvert.SerializeObject(cacheObject), cacheTimeSpan);

            _logger.Debug("Stored item with key: {0} in cache with timespan: {1}", cacheKey, cacheTimeSpan);
        }

        public void Remove<TCacheEntry, TFuncParam1>(TCacheEntry cacheEntry, TFuncParam1 funcParam1) where TCacheEntry : BaseCacheKey
        {
            var cacheKey = cacheEntry.Key(funcParam1);

            _logger.Debug("Removing item with key: {0} from cache", cacheKey);

            // todo any flags needed here??
            Cache.KeyDelete(cacheKey);

            _logger.Debug("Removed item with key: {0} from cache", cacheKey);
        }

        public void FlushAll()
        {
            _logger.Debug("Flushing cache");

            //todo correct implementation?
            Connection.GetServer(Connection.GetEndPoints().First()).FlushAllDatabases();

            _logger.Debug("Flushed cache");
        }
    }
}
