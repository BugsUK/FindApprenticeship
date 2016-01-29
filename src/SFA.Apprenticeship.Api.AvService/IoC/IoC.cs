namespace SFA.Apprenticeship.Api.AvService.IoC
{
    using Apprenticeships.Application.Interfaces.Organisations;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Application.Organisation;
    using Apprenticeships.Application.Provider;
    using Apprenticeships.Application.VacancyPosting;
    using Apprenticeships.Domain.Interfaces.Repositories.SFA.Apprenticeship.Api.AvService.Repositories;
    using Apprenticeships.Infrastructure.Common.IoC;
    using Apprenticeships.Infrastructure.EmployerDataService.IoC;
    using Apprenticeships.Infrastructure.Logging.IoC;
    using Apprenticeships.Infrastructure.Postcode.IoC;
    using Apprenticeships.Infrastructure.Repositories.Mongo.Providers.IoC;
    using Apprenticeships.Infrastructure.Repositories.Mongo.UserProfiles.IoC;
    using Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.IoC;
    using Apprenticeships.Infrastructure.Repositories.Sql.Configuration;
    using Apprenticeships.Infrastructure.Repositories.Sql.IoC;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.WebService;
    using Apprenticeships.Infrastructure.TacticalDataServices.IoC;
    using Infrastructure.Interfaces;
    using Mappers.Version51;
    using Mediators.Version51;
    using Providers;
    using Providers.Version51;
    using Repositories;
    using Services;
    using StructureMap;

    // NOTE: WCF IoC strategy is based on this article: https://lostechies.com/jimmybogard/2008/07/30/integrating-structuremap-with-wcf/.
    public static class IoC
    {
        static IoC()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();

            Container = new Container(x =>
            {
                // Core.
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<PostcodeRegistry>();

                // Repositories.
                x.AddRegistry<VacancyRepositoryRegistry>();
                x.AddRegistry<UserProfileRepositoryRegistry>();
                x.AddRegistry<ProviderRepositoryRegistry>();
                x.AddRegistry(new RepositoriesRegistry(sqlConfiguration));

                // Services.
                x.AddRegistry<EmployerDataServicesRegistry>();
                x.AddRegistry<TacticalDataServicesRegistry>();

                x.For<IVacancyPostingService>().Use<VacancyPostingService>();
                x.For<IProviderService>().Use<ProviderService>();
                x.For<IOrganisationService>().Use<OrganisationService>();

                // Web Service Mediators.
                x.For<IVacancyUploadServiceMediator>().Use<VacancyUploadServiceMediator>();
                x.For<IReferenceDataServiceMediator>().Use<ReferenceDataServiceMediator>();

                // Web Service Providers.
                x.For<IVacancyDetailsProvider>().Use<VacancyDetailsProvider>();
                x.For<IWebServiceAuthenticationProvider>().Use<WebServiceAuthenticationProvider>();
                x.For<IReferenceDataProvider>().Use<ReferenceDataProvider>();

                // Web Service Services.
                x.For<IWebServiceConsumerService>().Use<WebServiceConsumerService>();

                // Web Service Repositories.
                x.For<IWebServiceConsumerReadRepository>().Use<WebServiceConsumerRepository>();

                // Web Service Mappers.
                x.For<IAddressMapper>().Use<AddressMapper>();
                x.For<IApprenticeshipVacancyMapper>().Use<ApprenticeshipVacancyMapper>();
                x.For<IApprenticeshipVacancyQueryMapper>().Use<ApprenticeshipVacancyQueryMapper>();
                x.For<IVacancyDurationMapper>().Use<VacancyDurationMapper>();
                x.For<IVacancyUploadRequestMapper>().Use<VacancyUploadRequestMapper>();
                x.For<IWageMapper>().Use<WageMapper>();
                x.For<ICountyMapper>().Use<CountyMapper>();
            });
        }

        public static IContainer Container { get; private set; }
    }
}
