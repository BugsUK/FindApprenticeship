namespace FrameworkDataImport
{
    using Process;
    using SFA.Apprenticeships.Application.ReferenceData;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.IoC;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC;
    using SFA.Apprenticeships.Infrastructure.Logging.IoC;
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });
            var configurationService = container.GetInstance<IConfigurationService>();
            var cacheConfig = configurationService.Get<CacheConfiguration>();

            return new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(cacheConfig));
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();

                // cache service - to allow web site to run without azure cache
                x.AddCachingRegistry(cacheConfig);

                // service layer
                x.AddRegistry(new LegacyWebServicesRegistry(cacheConfig));

                x.For<IFrameworkDataComparer>().Use<FrameworkDataComparer>().Ctor<IReferenceDataProvider>().Named("LegacyReferenceDataProvider");
                x.For<IFrameworkDataLoader>().Use<FrameworkDataLoader>();
                x.For<IReferenceDataProvider>().Use<FrameworkDataReferenceDataProvider>().Name = "FrameworkDataReferenceDataProvider";
            });
        }
    }
}