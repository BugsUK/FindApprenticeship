namespace SFA.Apprenticeships.Infrastructure.Caching.Azure.IoC
{
    using Common.Configuration;
    using StructureMap.Configuration.DSL;
    using Domain.Interfaces.Caching;

    public class AzureCacheRegistry : Registry
    {
        public AzureCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<AzureCacheService>().Name = CacheConfiguration.AzureCacheName;
        }
    }
}
