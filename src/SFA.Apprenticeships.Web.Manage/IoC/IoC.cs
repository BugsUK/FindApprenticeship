namespace SFA.Apprenticeships.Web.Manage.IoC
{
    using Application.Application.IoC;
    using Application.Employer;
    using Application.Interfaces.Employers;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Providers;
    using Application.Interfaces.Users;
    using Application.Provider;
    using Application.UserAccount;
    using Application.UserProfile;
    using Application.VacancyPosting.IoC;
    using Common.IoC;
    using Common.Providers;
    using Common.Providers.Azure.AccessControlService;
    using Common.Services;
    using Infrastructure.Azure.ServiceBus.Configuration;
    using Infrastructure.Azure.ServiceBus.IoC;
    using Infrastructure.Caching.Memory.IoC;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.EmployerDataService.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.Postcode.IoC;
    using Infrastructure.Repositories.Mongo.Applications.IoC;
    using Infrastructure.Repositories.Mongo.Candidates.IoC;
    using Infrastructure.Repositories.Mongo.Employers.IoC;
    using Infrastructure.Repositories.Mongo.Providers.IoC;
    using Infrastructure.Repositories.Mongo.UserProfiles.IoC;
    using Infrastructure.Repositories.Sql.Configuration;
    using Infrastructure.Repositories.Sql.IoC;
    using Infrastructure.Repositories.Sql.Schemas.Vacancy.IoC;

    using SFA.Apprenticeships.Application.Interfaces;

    using StructureMap;
    using StructureMap.Web;
    using EuCookieDirectiveProvider = Raa.Common.Providers.EuCookieDirectiveProvider;
    using CookieDetectionProvider = Raa.Common.Providers.CookieDetectionProvider;

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
            var azureServiceBusConfiguration = configurationService.Get<AzureServiceBusConfiguration>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();

            return new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(cacheConfig));
                x.AddRegistry<LoggingRegistry>();

                // cache service - to allow web site to run without azure cache
                x.AddCachingRegistry(cacheConfig);

                // service layer
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<ProviderRepositoryRegistry>();
                x.AddRegistry<EmployerRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserProfileRepositoryRegistry>();
                x.AddRegistry<VacancyRepositoryRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry(new AzureServiceBusRegistry(azureServiceBusConfiguration));
                x.AddRegistry<ApplicationServicesRegistry>();
                x.AddRegistry(new RepositoriesRegistry(sqlConfiguration));
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry<AzureRedisCacheRegistry>();
                x.AddRegistry<VacancySourceRegistry>();
                x.AddRegistry<VacancyPostingServiceRegistry>();
                x.For<IProviderService>().Use<ProviderService>();
                x.For<IEmployerService>().Use<EmployerService>();
                x.For<IUserProfileService>().Use<UserProfileService>();
                x.For<IProviderUserAccountService>().Use<ProviderUserAccountService>();
                x.For<IProviderUserAccountService>().Use<ProviderUserAccountService>();
                x.For<IProviderVacancyAuthorisationService>().Use<ProviderVacancyAuthorisationService>();

                // web layer
                x.AddRegistry<WebCommonRegistry>();
                x.AddRegistry<ManagementWebRegistry>();

                x.For<IUserDataProvider>().HttpContextScoped().Use<CookieUserDataProvider>();
                x.For<IEuCookieDirectiveProvider>().Use<EuCookieDirectiveProvider>();
                x.For<ICookieDetectionProvider>().Use<CookieDetectionProvider>();
                x.For<IRobotCrawlerProvider>().Use<RobotCrawlerProvider>().Singleton();
                x.For<IDismissPlannedOutageMessageCookieProvider>().Use<DismissPlannedOutageMessageCookieProvider>();
                x.For<IHelpCookieProvider>().Use<HelpCookieProvider>();
                x.For<IAuthorizationErrorProvider>().Use<AuthorizationErrorProvider>();

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