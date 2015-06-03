namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using Caching.Azure.IoC;
    using Caching.Memory.IoC;
    using StructureMap;

    public static class ConfigurationExpressionExtension
    {
        public static void AddCachingRegistry(this ConfigurationExpression configurationExpression, CacheConfiguration cacheConfiguration)
        {
            switch (cacheConfiguration.DefaultCache)
            {
                case AzureCacheRegistry.AzureCacheName:
                    // Ordering of cache registration is important as last cache registered will be used by default. Memory cache is
                    // used selectively when running under Azure (e.g. to cache configuration).
                    configurationExpression.AddRegistry<MemoryCacheRegistry>();
                    configurationExpression.AddRegistry(new AzureCacheRegistry(cacheConfiguration.CacheName));
                    break;
                case MemoryCacheRegistry.MemoryCacheName:
                    configurationExpression.AddRegistry<MemoryCacheRegistry>();
                    break;
            }            
        }
    }
}
