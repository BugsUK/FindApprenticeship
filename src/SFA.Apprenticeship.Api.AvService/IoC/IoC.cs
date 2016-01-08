using SFA.Apprenticeships.Application.Interfaces.Organisations;
using SFA.Apprenticeships.Application.Interfaces.Providers;
using SFA.Apprenticeships.Application.Interfaces.VacancyPosting;
using SFA.Apprenticeships.Application.Organisation;
using SFA.Apprenticeships.Application.Provider;
using SFA.Apprenticeships.Application.VacancyPosting;
using SFA.Apprenticeships.Infrastructure.EmployerDataService.IoC;
using SFA.Apprenticeships.Infrastructure.Repositories.Providers.IoC;
using SFA.Apprenticeships.Infrastructure.Repositories.UserProfiles.IoC;
using SFA.Apprenticeships.Infrastructure.TacticalDataServices;
using SFA.Apprenticeships.Infrastructure.TacticalDataServices.IoC;

namespace SFA.Apprenticeship.Api.AvService.IoC
{
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
                x.For<IVacancyUploadRequestMapper>().Use<VacancyUploadRequestMapper>();
                x.For<IWageMapper>().Use<WageMapper>();
            });
        }

        public static IContainer Container { get; private set; }
    }
}
