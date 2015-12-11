namespace SFA.Apprenticeship.Api.AvService.IoC
{
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Application.VacancyPosting;
    using Apprenticeships.Infrastructure.Common.IoC;
    using Apprenticeships.Infrastructure.Logging.IoC;
    using Apprenticeships.Infrastructure.Repositories.Vacancies.IoC;
    using Mappers.Version51;
    using Providers.Version51;
    using StructureMap;

    // NOTE: WCF IoC strategy is based on this article: https://lostechies.com/jimmybogard/2008/07/30/integrating-structuremap-with-wcf/.
    public static class IoC
    {
        static IoC()
        {
            Container = new Container(x =>
            {
                // Core.
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommonRegistry>();

                // Repositories.
                x.AddRegistry<VacancyRepositoryRegistry>();

                // Services.
                x.For<IVacancyPostingService>().Use<VacancyPostingService>();

                // API Providers.
                x.For<IVacancyDetailsProvider>().Use<VacancyDetailsProvider>();
                x.For<IVacancyUploadProvider>().Use<VacancyUploadProvider>();

                // API Mappers.
                x.For<IAddressMapper>().Use<AddressMapper>();
                x.For<IApprenticeshipVacancyMapper>().Use<ApprenticeshipVacancyMapper>();
                x.For<IApprenticeshipVacancyQueryMapper>().Use<ApprenticeshipVacancyQueryMapper>();
                x.For<IVacancyDurationMapper>().Use<VacancyDurationMapper>();
                x.For<IVacancyUploadRequestMapper>().Use<VacancyUploadRequestMapper>();
                x.For<IWageMapper>().Use<WageMapper>();
            });
        }

        public static IContainer Container { get; private set; }
    }
}
