using SFA.Apprenticeships.Web.Recruit.Mediators.Home;
using SFA.Apprenticeships.Web.Recruit.Mediators.Provider;
using SFA.Apprenticeships.Web.Recruit.Providers;

namespace SFA.Apprenticeships.Web.Recruit.IoC {
    using Application.Interfaces.Logging;
    using Common.IoC;
    using Common.Providers;
    using Common.Services;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.EmployerDataService.IoC;
    using Infrastructure.Logging.IoC;
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

            return new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(cacheConfig));
                x.AddRegistry<LoggingRegistry>();

                // cache service - to allow web site to run without azure cache
                x.AddCachingRegistry(cacheConfig);

                // service layer
                x.AddRegistry<EmployerDataServicesRegistry>();

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

                x.For<IProviderProvider>().Use<ProviderProvider>();
                x.For<IProviderUserProvider>().Use<ProviderUserProvider>();

                x.For<IHomeMediator>().Use<HomeMediator>();
                x.For<IProviderMediator>().Use<ProviderMediator>();

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