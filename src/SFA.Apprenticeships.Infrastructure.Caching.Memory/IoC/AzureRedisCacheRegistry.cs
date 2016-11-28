namespace SFA.Apprenticeships.Infrastructure.Caching.Memory.IoC
{
    using Application.Interfaces.Caching;
    using StructureMap.Configuration.DSL;

    public class AzureRedisCacheRegistry : Registry
    {
        public const string AzureRedisCacheName = "AzureRedisCacheService";

        public AzureRedisCacheRegistry()
        {
            For<ICacheService>().Singleton().Use<AzureRedisCacheService>().Name = AzureRedisCacheName;
        }
    }
}