namespace SFA.Apprenticeship.Api.AvService.IoC
{
    using Apprenticeships.Application.Interfaces.Organisations;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Application.Organisation;
    using Apprenticeships.Application.Provider;
    using Apprenticeships.Application.VacancyPosting;
    using Apprenticeships.Infrastructure.Common.IoC;
    using Apprenticeships.Infrastructure.EmployerDataService.IoC;
    using Apprenticeships.Infrastructure.Logging.IoC;
    using Apprenticeships.Infrastructure.Repositories.Providers.IoC;
    using Apprenticeships.Infrastructure.Repositories.UserProfiles.IoC;
    using Apprenticeships.Infrastructure.Repositories.Vacancies.IoC;
    using Apprenticeships.Infrastructure.TacticalDataServices.IoC;
    using Mappers.Version51;
    using Providers.Version51;
    using StructureMap;
    using ApprenticeshipVacancyBuilder = Builders.Version51.ApprenticeshipVacancyBuilder;

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
                x.AddRegistry<UserProfileRepositoryRegistry>();
                x.AddRegistry<ProviderRepositoryRegistry>();

                // Services.
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<TacticalDataServicesRegistry>();

                x.For<IVacancyPostingService>().Use<VacancyPostingService>();
                x.For<IProviderService>().Use<ProviderService>();
                x.For<IOrganisationService>().Use<OrganisationService>();

                // API Providers.
                x.For<IVacancyDetailsProvider>().Use<VacancyDetailsProvider>();
                x.For<IVacancyUploadProvider>().Use<VacancyUploadProvider>();

                // API Mappers.
                x.For<IAddressMapper>().Use<AddressMapper>();
                x.For<IApprenticeshipVacancyMapper>().Use<ApprenticeshipVacancyMapper>();
                x.For<IApprenticeshipVacancyQueryMapper>().Use<ApprenticeshipVacancyQueryMapper>();
                x.For<IVacancyDurationMapper>().Use<VacancyDurationMapper>();
                x.For<ApprenticeshipVacancyBuilder>().Use<ApprenticeshipVacancyBuilder>();
                x.For<IWageMapper>().Use<WageMapper>();
            });
        }

        public static IContainer Container { get; private set; }
    }
}
