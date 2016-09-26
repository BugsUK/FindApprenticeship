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
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = new ConfigurationManager().GetAppSetting("CacheConnection");
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        private IDatabase _cache;
        private readonly object _locker = new object();
        private ILogService _logger;

        private const string GettingItemFromCacheFormat = "Redis: Getting item with key: {0} from cache";
        private const string ItemReturnedFromCacheFormat = "Redis: Item with key: {0} returned from cache";
        private const string ItemNotInCacheFormat = "Redis: Item with key: {0} not in cache";

        public static ConnectionMultiplexer Connection => lazyConnection.Value;

        public AzureRedisCacheService(ILogService logService)
        {
            _logger = logService;
           
            _cache = Connection.GetDatabase();
        }

        public T Get<T>(string key) where T : class
        {
            var value = _cache.StringGet(key);

            return JsonConvert.DeserializeObject<T>(value);
        }

        public TResult Get<TCacheEntry, TResult>(TCacheEntry cacheEntry, Func<TResult> dataFunc) where TCacheEntry : BaseCacheKey where TResult : class
        {
            var cacheKey = cacheEntry.Key();

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            //MemoryCache is thread safe however only the access is protected. The cache pattern of check then retrieve if null is not protected.
            //This allows multiple threads to execute the dataFunc uneccessarily. A lock here solves this issue
            
            lock (_locker)
            {
                var value = _cache.StringGet(cacheKey);
                TResult result = null;

                if (value.HasValue)
                {
                    result = JsonConvert.DeserializeObject<TResult>(value);
                }

                if (result == null || result.Equals(default(TResult)))
                {
                    _logger.Debug(ItemNotInCacheFormat, cacheKey);

                    var expiry = DateTime.Now.AddMinutes((int)cacheEntry.Duration);

                    result = dataFunc();
                    _cache.StringSet(cacheKey, JsonConvert.SerializeObject(result));
                    _cache.KeyExpire(cacheKey, expiry);

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
                var value = _cache.StringGet(cacheKey);
                TResult result = null;

                if (value.HasValue)
                {
                    result = JsonConvert.DeserializeObject<TResult>(value);
                }

                if (result == null || result.Equals(default(TResult)))
                {
                    _logger.Debug(ItemNotInCacheFormat, cacheKey);

                    var expiry = DateTime.Now.AddMinutes((int) cacheEntry.Duration);

                    result = dataFunc(funcParam1);
                    _cache.StringSet(cacheKey, JsonConvert.SerializeObject(result));
                    _cache.KeyExpire(cacheKey, expiry);

                    return result;
                }

                _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

                return result;
            }
        }

        public void PutObject(string cacheKey, object cacheObject, CacheDuration cacheDuration = CacheDuration.CacheDefault)
        {
            var expiry = DateTime.Now.AddMinutes((int)cacheDuration);

            _cache.StringSet(cacheKey, JsonConvert.SerializeObject(cacheObject));
            _cache.KeyExpire(cacheKey, expiry);
        }

        public void Remove<TCacheEntry, TFuncParam1>(TCacheEntry cacheEntry, TFuncParam1 funcParam1) where TCacheEntry : BaseCacheKey
        {
            var cacheKey = cacheEntry.Key(funcParam1);

            // todo any flags needed here??
            _cache.KeyDelete(cacheKey);
        }

        public void FlushAll()
        {
            //todo correct implementation?
            Connection.GetServer(Connection.GetEndPoints().First()).FlushAllDatabases();
        }
    }
}
