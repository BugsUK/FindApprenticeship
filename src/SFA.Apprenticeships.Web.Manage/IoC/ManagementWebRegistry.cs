namespace SFA.Apprenticeships.Web.Manage.IoC
{
    using System.Web;
    using Application.Candidate;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Candidates;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Employer.Strategies;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Organisations;
    using Application.Interfaces.Users;
    using Application.Organisation;
    using Application.UserAccount;
    using Application.UserAccount.Strategies.ProviderUserAccount;
    using Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Mediators.AgencyUser;
    using Mediators.Vacancy;
    using Providers;
    using Raa.Common.Mappers;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using Application.Interfaces.ReferenceData;
    using Application.Location;
    using Application.ReferenceData;
    using Application.VacancyPosting.Strategies;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Raa.Mappers;
    using Infrastructure.Raa.Strategies;
    using Infrastructure.Repositories.Sql.Schemas.dbo;
    using Mediators.Candidate;
    using Mediators.InformationRadiator;
    using Mediators.Reporting;
    using Raa.Common.Providers;
    using Application.Interfaces;
    using Application.Interfaces.Security;
    using Application.Vacancy;
    using Infrastructure.Security;
    using Raa.Common.Mediators.Admin;
    using Raa.Common.ViewModels.Application;

    public class ManagementWebRegistry : Registry
    {
        public ManagementWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            For<IMapper>().Singleton().Use<RaaCommonWebMappers>().Name = "RaaCommonWebMappers";
            For<IMapper>().Singleton().Use<Raa.Common.Mappers.CandidateMappers>().Name = "CandidateMappers";

            RegisterCodeGenerators();
            RegisterServices();
            RegisterStrategies();
            RegisterProviders();
            RegisterMediators();
            RegisterRepositories();
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
            For<IVacancyPostingProvider>().Use<VacancyProvider>().Ctor<IMapper>().Named("RaaCommonWebMappers");
            For<IProviderQAProvider>().Use<ProviderProvider>();
            For<ILocationsProvider>().Use<LocationsProvider>();
            For<ICandidateProvider>().Use<CandidateProvider>().Ctor<IMapper>().Named("CandidateMappers");
            For<IGeoCodingProvider>().Use<GeoCodingProvider>();
            For<IEncryptionProvider>().Use<AES256Provider>();
            For<IProviderProvider>().Use<ProviderProvider>();
            For<IApiUserProvider>().Use<ApiUserProvider>();
            For<IProviderUserProvider>().Use<ProviderUserProvider>();
        }

        private void RegisterServices()
        {
            For<IGeoCodeLookupService>().Use<GeoCodeLookupService>();
            For<IOrganisationService>().Use<OrganisationService>();
            For<IReferenceDataService>().Use<ReferenceDataService>();
            For<IProviderCommunicationService>().Use<ProviderCommunicationService>();
            For<IEmployerCommunicationService>().Use<EmployerCommunicationService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<ICandidateSearchService>().Use<CandidateSearchService>();
            For<ICandidateApplicationService>().Use<CandidateApplicationService>();
            For<IGeoCodeLookupService>().Use<GeoCodeLookupService>();
            For<ILocalAuthorityLookupService>().Use<LocalAuthorityLookupService>();
            For<IEncryptionService<AnonymisedApplicationLink>>().Use<CryptographyService<AnonymisedApplicationLink>>();
            For<IVacancySummaryService>().Use<VacancySummaryService>();
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
            For<ISendEmployerCommunicationStrategy>().Use<QueueEmployerCommunicationStrategy>();
            For<ISendEmailVerificationCodeStrategy>().Use<SendEmailVerificationCodeStrategy>()
                .Ctor<ICodeGenerator>().Named(codeGenerator);
            For<IResendEmailVerificationCodeStrategy>().Use<ResendEmailVerificationCodeStrategy>();
            For<IGetCandidateByIdStrategy>().Use<GetCandidateByIdStrategy>();
            For<IGetCandidateSummariesStrategy>().Use<GetCandidateSummariesStrategy>();

            For<IGetByIdStrategy>().Use<GetByIdStrategy>();
            For<IGetByIdsStrategy>().Use<GetByIdsStrategy>();
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<IGetPagedEmployerSearchResultsStrategy>().Use<GetPagedEmployerSearchResultsStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<ISaveEmployerStrategy>().Use<SaveEmployerStrategy>();
            For<ISendEmployerLinksStrategy>().Use<SendEmployerLinksStrategy>();

            For<IPublishVacancySummaryUpdateStrategy>().Use<PublishVacancySummaryUpdateStrategy>().Ctor<IMapper>().Is<VacancySummaryUpdateMapper>();
            For<IGetReleaseNotesStrategy>().Use<GetReleaseNotesStrategy>();

            For<ISearchCandidatesStrategy>().Use<SearchCandidatesStrategy>().Ctor<ICandidateReadRepository>().Is<CandidateRepository>();

            For<Application.UserAccount.Strategies.ProviderUserAccount.ISubmitContactMessageStrategy>().Use<Application.UserAccount.Strategies.ProviderUserAccount.SubmitContactMessageStrategy>();
        }

        private void RegisterMediators()
        {
            For<IAgencyUserMediator>().Use<AgencyUserMediator>();
            For<ICandidateMediator>().Use<CandidateMediator>();
            For<IVacancyMediator>().Use<VacancyMediator>();
            For<IReportingMediator>().Use<ReportingMediator>();
            For<IInformationRadiatorMediator>().Use<InformationRadiatorMediator>();
            For<IAdminMediator>().Use<AdminMediator>();
        }

        private void RegisterRepositories()
        {
            For<IApprenticeshipApplicationStatsRepository>().Use<ApplicationStatsRepository>();
            For<ITraineeshipApplicationStatsRepository>().Use<ApplicationStatsRepository>();
        }
    }
}
