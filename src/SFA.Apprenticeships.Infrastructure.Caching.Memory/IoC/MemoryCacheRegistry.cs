namespace SFA.Apprenticeships.Infrastructure.Caching.Memory.IoC
{
    using Common.Configuration;
    using StructureMap.Configuration.DSL;
    using Domain.Interfaces.Caching;

    public class MemoryCacheRegistry : Registry
    {
        public MemoryCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<MemoryCacheService>().Name = CacheConfiguration.MemoryCacheName;
        }
    }
}
