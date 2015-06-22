namespace SFA.Apprenticeships.Infrastructure.Common.IoC
{
    using Caching.Memory.IoC;
    using Configuration;
    using Domain.Interfaces.Caching;
    using Domain.Interfaces.Configuration;
    using StructureMap.Configuration.DSL;

    public class CommonRegistry : Registry
    {
        public CommonRegistry() : this(new CacheConfiguration()) { }

        public CommonRegistry(CacheConfiguration cacheConfiguration)
        {
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
            For<IConfigurationService>().Singleton().Use<ConfigurationService>().Name = "ConfigurationService";

            if (cacheConfiguration.UseCache)
            {
                For<IConfigurationService>()
                    .Singleton()
                    .Use<CachedConfigurationService>()
                    .Ctor<IConfigurationService>()
                    .Named("ConfigurationService")
                    .Ctor<ICacheService>()
                    .Named(MemoryCacheRegistry.MemoryCacheName);
            }
        }
    }
}
