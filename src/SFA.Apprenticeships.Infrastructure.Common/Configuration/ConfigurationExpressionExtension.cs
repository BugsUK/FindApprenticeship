namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using Caching.Azure.IoC;
    using Caching.Memory.IoC;
    using StructureMap;

    public static class ConfigurationExpressionExtension
    {
        public static void AddCachingRegistry(this ConfigurationExpression configurationExpression, CacheConfiguration cacheConfiguration)
        {
            // either one of the other is used
            switch (cacheConfiguration.DefaultCache)
            {
                case AzureCacheRegistry.AzureCacheName:
                    configurationExpression.AddRegistry(new AzureCacheRegistry(cacheConfiguration.CacheName));
                    break;
                case MemoryCacheRegistry.MemoryCacheName:
                    configurationExpression.AddRegistry<MemoryCacheRegistry>();
                    break;
            }            
        }
    }
}
