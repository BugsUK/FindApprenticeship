namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using Caching.Memory.IoC;
    using StructureMap;

    //TODO this should be moved so Caching.Memory doesn't need to reference Common
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
