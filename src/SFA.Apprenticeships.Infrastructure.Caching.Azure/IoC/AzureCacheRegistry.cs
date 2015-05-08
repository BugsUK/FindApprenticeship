namespace SFA.Apprenticeships.Infrastructure.Caching.Azure.IoC
{
    using StructureMap.Configuration.DSL;
    using Domain.Interfaces.Caching;

    public class AzureCacheRegistry : Registry
    {
        public const string AzureCacheName = "AzureCacheService";

        public AzureCacheRegistry(string cacheName)
        {
            For<ICacheService>().Singleton().Use<AzureCacheService>().Ctor<string>().Is(cacheName).Name = AzureCacheName;
        }
    }
}
