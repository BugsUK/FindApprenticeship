namespace SFA.Apprenticeships.Web.Recruit.IoC
{
    using System;
    using System.Linq.Expressions;
    using System.Web;
    using Application.Application;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Providers;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Application.Provider;
    using Application.UserAccount;
    using Application.UserProfile;
    using Application.VacancyPosting;
    using Common.IoC;
    using Common.Providers;
    using Common.Providers.Azure.AccessControlService;
    using Common.Services;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Azure.ServiceBus.IoC;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.EmployerDataService.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.Postcode.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Employers.IoC;
    using Infrastructure.Repositories.Providers.IoC;
    using Infrastructure.Repositories.UserProfiles.IoC;
    using Infrastructure.Repositories.Vacancies.IoC;
    using Infrastructure.TacticalDataServices.IoC;
    using StructureMap;
    using StructureMap.Web;
    using StructureMap.Web.Pipeline;

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

                //// cache service - to allow web site to run without azure cache
                x.AddCachingRegistry(cacheConfig);

                //// service layer
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<ProviderRepositoryRegistry>();
                x.AddRegistry<EmployerRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserProfileRepositoryRegistry>();
                x.AddRegistry<VacancyRepositoryRegistry>();
                x.AddRegistry<AzureServiceBusRegistry>();
                x.AddRegistry<TacticalDataServicesRegistry>();
                x.AddRegistry<PostcodeRegistry>();

                x.For<IProviderService>().Use<ProviderService>();
                x.For<IUserProfileService>().Use<UserProfileService>();
                x.For<IProviderUserAccountService>().Use<ProviderUserAccountService>();
                x.For<IVacancyPostingService>().Use<VacancyPostingService>();
                x.For<IApplicationService>().Use<ApplicationService>();

                // web layer
                x.AddRegistry<WebCommonRegistry>();
                x.AddRegistry<RecruitmentWebRegistry>();

                x.For<IUserDataProvider>().HttpContextScoped().Use<CookieUserDataProvider>();
                x.For<IEuCookieDirectiveProvider>().Use<EuCookieDirectiveProvider>();
                x.For<ICookieDetectionProvider>().Use<CookieDetectionProvider>();
                x.For<IRobotCrawlerProvider>().Use<RobotCrawlerProvider>().Singleton();
                x.For<IDismissPlannedOutageMessageCookieProvider>().Use<DismissPlannedOutageMessageCookieProvider>();
                x.For<IHelpCookieProvider>().Use<HelpCookieProvider>();
                x.For<ICookieAuthorizationDataProvider>().Use<CookieAuthorizationDataProvider>();
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
                x.Policies.SetAllProperties(y => y.OfType<ICookieAuthorizationDataProvider>());
            });
        }
    }
}
