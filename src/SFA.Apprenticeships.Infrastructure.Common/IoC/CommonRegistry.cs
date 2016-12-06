namespace SFA.Apprenticeships.Infrastructure.Common.IoC
{
    using SFA.Infrastructure.Interfaces;
    using Configuration;
    using CurrentUser;
    using DateTime;
    using Application.Interfaces;
    using Application.Interfaces.Caching;
    using SFA.Infrastructure.Azure.Configuration;
    using SFA.Infrastructure.Configuration;
    using StructureMap.Configuration.DSL;

    public class CommonRegistry : Registry
    {
        public CommonRegistry() : this(new CacheConfiguration()) { }

        public CommonRegistry(CacheConfiguration cacheConfiguration)
        {
            For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
            For<IConfigurationService>().Singleton().Use<AzureBlobConfigurationService>().Name = "ConfigurationService";
            For<IDateTimeService>().Use<DateTimeService>();
            For<ICurrentUserService>().Use<CurrentUserService>();

            if (cacheConfiguration.UseCache)
            {
                For<IConfigurationService>()
                    .Singleton()
                    .Use<CachedConfigurationService>()
                    .Ctor<IConfigurationService>()
                    .Named("ConfigurationService")
                    .Ctor<ICacheService>()
                    .Named(cacheConfiguration.DefaultCache);
            }
        }
    }
}
