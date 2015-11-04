namespace SFA.Apprenticeships.Infrastructure.Common.IoC
{
    using Application.Interfaces.DateTime;
    using Caching.Memory.IoC;
    using Configuration;
    using DateTime;
    using Domain.Interfaces.Caching;
    using Domain.Interfaces.Configuration;
    using StructureMap.Configuration.DSL;

    public class CommonRegistry : Registry
    {
        public CommonRegistry() : this(new CacheConfiguration()) { }

        public CommonRegistry(CacheConfiguration cacheConfiguration)
        {
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
            For<IConfigurationService>().Singleton().Use<AzureBlobConfigurationService>().Name = "ConfigurationService";
            For<IDateTimeService>().Use<DateTimeService>();

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
