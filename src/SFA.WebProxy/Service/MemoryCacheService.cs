namespace SFA.WebProxy.Service
{
    using System;
    using System.Runtime.Caching;

    public class MemoryCacheService : ICacheService
    {
        private readonly object _locker = new object();
        private readonly int _cacheDurationInSeconds;
        private readonly MemoryCache _memoryCache;

        public MemoryCacheService(int cacheDurationInSeconds)
        {
            _cacheDurationInSeconds = cacheDurationInSeconds;
            _memoryCache = MemoryCache.Default;
        }

        public T Get<T>(string key, Func<T> valueFunc)
        {
            //MemoryCache is thread safe however only the access is protected. The cache pattern of check then retrieve if null is not protected.
            //This allows multiple threads to execute the dataFunc uneccessarily. A lock here solves this issue
            lock (_locker)
            {
                var value = _memoryCache[key];
                if (value == null || value.Equals(default(T)))
                {
                    value = valueFunc();
                    var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(_cacheDurationInSeconds) };
                    _memoryCache.Set(key, value, policy);
                    return (T)value;
                }
                return (T)value;
            }
        }
    }
}