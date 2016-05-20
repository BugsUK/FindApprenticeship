namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using Caching.Memory.IoC;
    using StructureMap;

    public static class ConfigurationExpressionExtension
    {
        public static void AddCachingRegistry(this ConfigurationExpression configurationExpression, CacheConfiguration cacheConfiguration)
        {
            switch (cacheConfiguration.DefaultCache)
            {
                case MemoryCacheRegistry.MemoryCacheName:
                    configurationExpression.AddRegistry<MemoryCacheRegistry>();
                    break;
            }            
        }
    }
}
