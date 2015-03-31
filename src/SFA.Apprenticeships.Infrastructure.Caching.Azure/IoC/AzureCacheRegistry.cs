namespace SFA.Apprenticeships.Infrastructure.Caching.Azure.IoC
{
    using StructureMap.Configuration.DSL;
    using Domain.Interfaces.Caching;

    public class AzureCacheRegistry : Registry
    {
        public const string AzureCacheName = "AzureCacheService";

        public AzureCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<AzureCacheService>().Name = AzureCacheName;
        }
    }
}
