namespace SFA.Apprenticeships.Infrastructure.Caching.Memory.IoC
{
    using StructureMap.Configuration.DSL;
    using Domain.Interfaces.Caching;

    public class MemoryCacheRegistry : Registry
    {
        public const string MemoryCacheName = "MemoryCacheService";

        public MemoryCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<MemoryCacheService>().Name = MemoryCacheName;
        }
    }
}
