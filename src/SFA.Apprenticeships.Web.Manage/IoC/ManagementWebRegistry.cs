namespace SFA.Apprenticeships.Web.Manage.IoC
{
    using System.Web;
    using Application.Candidate;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Candidate.Strategies.Traineeships;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Employer.Strategies;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Organisations;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Application.Organisation;
    using Application.UserAccount;
    using Application.UserAccount.Strategies.ProviderUserAccount;
    using Application.VacancyPosting;
    using Common.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Mediators.AgencyUser;
    using Mediators.Vacancy;
    using Providers;
    using Raa.Common.Mappers;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Reporting;
    using Application.Interfaces.Vacancies;
    using Application.Location;
    using Application.ReferenceData;
    using Application.Reporting;
    using Mappers;
    using Mediators.Candidate;
    using Mediators.Reporting;
    using Raa.Common.Providers;

    public class ManagementWebRegistry : Registry
    {
        public ManagementWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            For<IMapper>().Singleton().Use<RaaCommonWebMappers>().Name = "RaaCommonWebMappers";
            For<IMapper>().Singleton().Use<CandidateMappers>().Name = "CandidateMappers";

            RegisterCodeGenerators();
            RegisterServices();
            RegisterStrategies();
            RegisterProviders();
            RegisterMediators();
        }

        private void RegisterCodeGenerators()
        {
            For<ICodeGenerator>().Use<RandomCodeGenerator>().Name = "RandomCodeGenerator";
            For<ICodeGenerator>().Use<StaticCodeGenerator>().Name = "StaticCodeGenerator";
        }

        private void RegisterProviders()
        {
            For<IAgencyUserProvider>().Use<AgencyUserProvider>();
            For<IVacancyQAProvider>().Use<VacancyProvider>().Ctor<IMapper>().Named("RaaCommonWebMappers");
            For<IProviderQAProvider>().Use<ProviderProvider>();
            For<ILocationsProvider>().Use<LocationsProvider>();
            For<ICandidateProvider>().Use<CandidateProvider>().Ctor<IMapper>().Named("CandidateMappers");
            For<IGeoCodingProvider>().Use<GeoCodingProvider>();
        }

        private void RegisterServices()
        {
            For<IGeoCodeLookupService>().Use<GeoCodeLookupService>();
            For<IOrganisationService>().Use<OrganisationService>();
            For<IReferenceDataService>().Use<ReferenceDataService>();
            For<IReportingService>().Use<ReportingService>();
            For<IProviderCommunicationService>().Use<ProviderCommunicationService>();
            For<IVacancyPostingService>().Use<VacancyPostingService>();
            For<IVacancyLockingService>().Use<VacancyLockingService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<ICandidateSearchService>().Use<CandidateSearchService>();
            For<ICandidateApplicationService>().Use<CandidateApplicationService>();
            For<IGeoCodeLookupService>().Use<GeoCodeLookupService>();
            For<ILocalAuthorityLookupService>().Use<LocalAuthorityLookupService>();
        }

        private void RegisterStrategies()
        {
            var settingsContainer = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommonRegistry>();
            });

            var configurationService = settingsContainer.GetInstance<IConfigurationService>();
            var codeGenerator = configurationService.Get<CommonWebConfiguration>().CodeGenerator;

            For<ISendProviderUserCommunicationStrategy>().Use<QueueProviderUserCommunicationStrategy>();
            For<ISendEmailVerificationCodeStrategy>().Use<SendEmailVerificationCodeStrategy>()
                .Ctor<ICodeGenerator>().Named(codeGenerator);
            For<IResendEmailVerificationCodeStrategy>().Use<ResendEmailVerificationCodeStrategy>();
            For<IGetCandidateByIdStrategy>().Use<GetCandidateByIdStrategy>();
            For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<GetCandidateApprenticeshipApplicationsStrategy>();
            For<IGetCandidateTraineeshipApplicationsStrategy>().Use<GetCandidateTraineeshipApplicationsStrategy>();

            For<IGetByIdStrategy>().Use<GetByIdStrategy>();
            For<IGetByIdsStrategy>().Use<GetByIdsStrategy>();
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<IGetPagedEmployerSearchResultsStrategy>().Use<GetPagedEmployerSearchResultsStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<ISaveEmployerStrategy>().Use<SaveEmployerStrategy>();
        }

        private void RegisterMediators()
        {
            For<IAgencyUserMediator>().Use<AgencyUserMediator>();
            For<ICandidateMediator>().Use<CandidateMediator>();
            For<IVacancyMediator>().Use<VacancyMediator>();
            For<IReportingMediator>().Use<ReportingMediator>();
        }
    }
}
