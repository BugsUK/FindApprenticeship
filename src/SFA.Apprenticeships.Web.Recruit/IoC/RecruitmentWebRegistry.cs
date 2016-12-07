namespace SFA.Apprenticeships.Web.Recruit.IoC
{
    using Application.Candidate;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Candidate.Strategies.Candidates;
    using Application.Candidate.Strategies.Traineeships;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Employer;
    using Application.Employer.Strategies;
    using Application.Interfaces;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Organisations;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Reporting;
    using Application.Interfaces.Security;
    using Application.Interfaces.Users;
    using Application.Location;
    using Application.Organisation;
    using Application.Provider;
    using Application.ReferenceData;
    using Application.Reporting;
    using Application.UserAccount;
    using Application.UserAccount.Strategies.ProviderUserAccount;
    using Application.Vacancy;
    using Application.VacancyPosting.Strategies;
    using Common.Configuration;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.Raa.Mappers;
    using Infrastructure.Raa.Strategies;
    using Infrastructure.Repositories.Sql.Schemas.dbo;
    using Infrastructure.Security;
    using Mappers;
    using Mediators.Application;
    using Mediators.Candidate;
    using Mediators.Home;
    using Mediators.Provider;
    using Mediators.ProviderUser;
    using Mediators.Report;
    using Mediators.VacancyManagement;
    using Mediators.VacancyPosting;
    using Mediators.VacancyStatus;
    using Raa.Common.Mappers;
    using Raa.Common.Mediators.Admin;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Application;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using System.Web;
    using CandidateRepository = Infrastructure.Repositories.Mongo.Candidates.CandidateRepository;
    using ISubmitContactMessageStrategy = Application.UserAccount.Strategies.ProviderUserAccount.ISubmitContactMessageStrategy;
    using SubmitContactMessageStrategy = Application.UserAccount.Strategies.ProviderUserAccount.SubmitContactMessageStrategy;

    public class RecruitmentWebRegistry : Registry
    {
        public RecruitmentWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            For<IMapper>().Singleton().Use<RaaCommonWebMappers>().Name = "RaaCommonWebMappers";
            For<IMapper>().Singleton().Use<RecruitMappers>().Name = "RecruitMappers";
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
            For<IProviderProvider>().Use<ProviderProvider>();
            For<IEmployerProvider>().Use<EmployerProvider>();
            For<IVacancyPostingProvider>().Use<VacancyProvider>().Ctor<IMapper>().Named("RaaCommonWebMappers");
            For<IVacancyManagementProvider>().Use<VacancyManagementProvider>();
            For<IProviderUserProvider>().Use<ProviderUserProvider>();
            For<IProviderMediator>().Use<ProviderMediator>();
            For<IApplicationProvider>().Use<ApplicationProvider>().Ctor<IMapper>().Named("RecruitMappers");
            For<ILocationsProvider>().Use<LocationsProvider>();
            For<IGeoCodingProvider>().Use<GeoCodingProvider>();
            For<IReportingProvider>().Use<ReportingProvider>();
            For<IEncryptionProvider>().Use<AES256Provider>();
            For<IVacancyStatusChangeProvider>().Use<VacancyStatusChangeProvider>();
            For<ICandidateProvider>().Use<CandidateProvider>().Ctor<IMapper>().Named("CandidateMappers");
            For<IApiUserProvider>().Use<ApiUserProvider>();
            For<IVacancyQAProvider>().Use<VacancyProvider>();
        }

        private void RegisterServices()
        {
            For<IGeoCodeLookupService>().Use<GeoCodeLookupService>();
            For<IOrganisationService>().Use<OrganisationService>();
            For<IProviderCommunicationService>().Use<ProviderCommunicationService>();
            For<IEmployerCommunicationService>().Use<EmployerCommunicationService>();
            For<IReferenceDataService>().Use<ReferenceDataService>();
            For<IProviderService>().Use<ProviderService>();
            For<IEmployerService>().Use<EmployerService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<IGeoCodeLookupService>().Use<GeoCodeLookupService>();
            For<ILocalAuthorityLookupService>().Use<LocalAuthorityLookupService>();
            For<ICommunicationService>().Use<CommunicationService>();
            For<IReportingService>().Use<ReportingService>();
            For<IEncryptionService<AnonymisedApplicationLink>>().Use<CryptographyService<AnonymisedApplicationLink>>();
            For<IDecryptionService<AnonymisedApplicationLink>>().Use<CryptographyService<AnonymisedApplicationLink>>();
            For<IVacancyManagementService>().Use<VacancyManagementService>();
            For<ICandidateApplicationService>().Use<CandidateApplicationService>();
            For<ICandidateSearchService>().Use<CandidateSearchService>();
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

            For<IGetByIdStrategy>().Use<GetByIdStrategy>();
            For<IGetByIdsStrategy>().Use<GetByIdsStrategy>();
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<ISearchEmployersStrategy>().Use<SearchEmployersStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<ISaveEmployerStrategy>().Use<SaveEmployerStrategy>();
            For<ISendEmployerLinksStrategy>().Use<SendEmployerLinksStrategy>();
            For<ISubmitContactMessageStrategy>().Use<SubmitContactMessageStrategy>();

            For<IPublishVacancySummaryUpdateStrategy>().Use<PublishVacancySummaryUpdateStrategy>().Ctor<IMapper>().Is<VacancySummaryUpdateMapper>();
            For<IGetReleaseNotesStrategy>().Use<GetReleaseNotesStrategy>();
            For<IDeleteVacancyStrategy>().Use<DeleteVacancyStrategy>();

            For<IMapper>().Singleton().Use<Infrastructure.Repositories.Mongo.Candidates.Mappers.CandidateMappers>().Name = "MongoCandidateMapper";
            For<ICandidateReadRepository>().Use<CandidateRepository>().Ctor<IMapper>().Named("MongoCandidateMapper");
            For<IGetCandidateByIdStrategy>().Use<GetCandidateByIdStrategy>();
            For<IGetCandidateSummariesStrategy>().Use<GetCandidateSummariesStrategy>();
            For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<GetCandidateApprenticeshipApplicationsStrategy>();
            For<IGetCandidateTraineeshipApplicationsStrategy>().Use<GetCandidateTraineeshipApplicationsStrategy>();

            For<ISearchCandidatesStrategy>().Use<SearchCandidatesStrategy>().Ctor<ICandidateReadRepository>().Is<Infrastructure.Repositories.Sql.Schemas.dbo.CandidateRepository>();
        }

        private void RegisterMediators()
        {
            For<IProviderMediator>().Use<ProviderMediator>();
            For<IProviderUserMediator>().Use<ProviderUserMediator>().Ctor<IMapper>().Named("RecruitMappers");
            For<IVacancyPostingMediator>().Use<VacancyPostingMediator>();
            For<IVacancyStatusMediator>().Use<VacancyStatusMediator>();
            For<IApplicationMediator>().Use<ApplicationMediator>();
            For<IApprenticeshipApplicationMediator>().Use<ApprenticeshipApplicationMediator>();
            For<ITraineeshipApplicationMediator>().Use<TraineeshipApplicationMediator>();
            For<IHomeMediator>().Use<HomeMediator>();
            For<IReportMediator>().Use<ReportMediator>();
            For<IVacancyManagementMediator>().Use<VacancyManagementMediator>();
            For<ICandidateMediator>().Use<CandidateMediator>();
            For<IAdminMediator>().Use<AdminMediator>();
        }

        private void RegisterRepositories()
        {
            For<IApprenticeshipApplicationStatsRepository>().Use<ApplicationStatsRepository>();
            For<ITraineeshipApplicationStatsRepository>().Use<ApplicationStatsRepository>();
        }
    }
}