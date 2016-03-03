namespace SFA.Apprenticeships.Web.Candidate.IoC {

    using SFA.Infrastructure.Interfaces;
    using Common.IoC;
    using Common.Providers;
    using Common.Services;
    using Infrastructure.Azure.ServiceBus.Configuration;
    using Infrastructure.Azure.ServiceBus.IoC;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.EmployerDataService.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.LocationLookup.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.Postcode.IoC;
    using Infrastructure.Raa.IoC;
    using Infrastructure.Repositories.Mongo.Applications.IoC;
    using Infrastructure.Repositories.Mongo.Audit.IoC;
    using Infrastructure.Repositories.Mongo.Authentication.IoC;
    using Infrastructure.Repositories.Mongo.Candidates.IoC;
    using Infrastructure.Repositories.Mongo.Communication.IoC;
    using Infrastructure.Repositories.Mongo.Employers.IoC;
    using Infrastructure.Repositories.Mongo.Providers.IoC;
    using Infrastructure.Repositories.Mongo.Users.IoC;
    using Infrastructure.Repositories.Mongo.Vacancies.IoC;
    using Infrastructure.Repositories.Sql.Configuration;
    using Infrastructure.Repositories.Sql.IoC;
    using Infrastructure.TacticalDataServices.IoC;
    using Infrastructure.UserDirectory.IoC;
    using Infrastructure.VacancySearch.IoC;
    using StructureMap;
    using StructureMap.Web;

    public static class IoC {
        public static IContainer Initialize()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });
            var configurationService = container.GetInstance<IConfigurationService>();
            var cacheConfig = configurationService.Get<CacheConfiguration>();
            var servicesConfiguration = configurationService.Get<ServicesConfiguration>();
            var azureServiceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();

            return new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(cacheConfig));
                x.AddRegistry<LoggingRegistry>();

                // cache service - to allow web site to run without azure cache
                x.AddCachingRegistry(cacheConfig);

                // service layer
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(cacheConfig, servicesConfiguration));
                x.AddRegistry(new RaaRegistry(servicesConfiguration));
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry(new AzureServiceBusRegistry(azureServiceBusConfiguration));
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<AuthenticationRepositoryRegistry>();
                x.AddRegistry<CommunicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<AuditRepositoryRegistry>();
                x.AddRegistry<VacancyRepositoryRegistry>();
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<TacticalDataServicesRegistry>();
                x.AddRegistry<ProviderRepositoryRegistry>();
                x.AddRegistry<EmployerRepositoryRegistry>();
                x.AddRegistry(new RepositoriesRegistry(sqlConfiguration));

                // web layer
                x.AddRegistry<WebCommonRegistry>();
                x.AddRegistry<CandidateWebRegistry>();

                x.For<IUserDataProvider>().HttpContextScoped().Use<CookieUserDataProvider>();
                x.For<IEuCookieDirectiveProvider>().Use<EuCookieDirectiveProvider>();
                x.For<ICookieDetectionProvider>().Use<CookieDetectionProvider>();
                x.For<IRobotCrawlerProvider>().Use<RobotCrawlerProvider>().Singleton();
                x.For<IDismissPlannedOutageMessageCookieProvider>().Use<DismissPlannedOutageMessageCookieProvider>();
                x.For<IHelpCookieProvider>().Use<HelpCookieProvider>();

                x.Policies.SetAllProperties(y => y.OfType<IConfigurationService>());
                x.Policies.SetAllProperties(y => y.OfType<ICookieDetectionProvider>());
                x.Policies.SetAllProperties(y => y.OfType<IEuCookieDirectiveProvider>());
                x.Policies.SetAllProperties(y => y.OfType<IRobotCrawlerProvider>());
                x.Policies.SetAllProperties(y => y.OfType<IUserDataProvider>());
                x.Policies.SetAllProperties(y => y.OfType<ILogService>());
                x.Policies.SetAllProperties(y => y.OfType<IDismissPlannedOutageMessageCookieProvider>());
                x.Policies.SetAllProperties(y => y.OfType<IHelpCookieProvider>());
                x.Policies.SetAllProperties(y => y.OfType<IAuthenticationTicketService>());
            });
        }
    }
}