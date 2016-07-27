namespace SFA.Apprenticeships.Infrastructure.Processes.IoC
{
    using Application.Applications;
    using Application.Applications.Entities;
    using Application.Applications.Housekeeping;
    using Application.Applications.Strategies;
    using Application.Candidate;
    using Application.Candidates.Entities;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Communications.Housekeeping;
    using Application.Employer;
    using Application.Employer.Strategies;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Organisations;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Location;
    using Application.Organisation;
    using Application.Provider;
    using Application.ReferenceData;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Application.Vacancies.Entities.SiteMap;
    using Application.Vacancy.SiteMap;
    using Applications;
    using Azure.ServiceBus;
    using Candidates;
    using Common.IoC;
    using Communication.Configuration;
    using Communications;
    using Communications.Commands;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Logging.IoC;
    using Repositories.Mongo.Audit;
    using SiteMap;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using Vacancies;

    public class ProcessesRegistry : Registry
    {
        public ProcessesRegistry()
        {
            // communications
            var container = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommonRegistry>();
            });

            For<CommunicationCommand>().Use<CandidateCommunicationCommand>();
            For<CommunicationCommand>().Use<CandidateDailyDigestCommunicationCommand>();
            For<CommunicationCommand>().Use<HelpDeskCommunicationCommand>();
            For<CommunicationCommand>().Use<ProviderCommunicationCommand>();

            For<ISendApplicationSubmittedStrategy>().Use<LegacyQueueApprenticeshipApplicationSubmittedStrategy>();
            For<ISendTraineeshipApplicationSubmittedStrategy>().Use<LegacyQueueTraineeshipApplicationSubmittedStrategy>();
            For<ISendCandidateCommunicationStrategy>().Use<QueueCandidateCommunicationStrategy>();
            For<ISendContactMessageStrategy>().Use<QueueContactMessageStrategy>();
            For<ISendUsernameUpdateCommunicationStrategy>().Use<QueueUsernameUpdateCommunicationStrategy>();

            For<ICommunicationService>().Use<CommunicationService>();

            // applications
            For<IApplicationStatusProcessor>().Use<ApplicationStatusProcessor>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();

            // vacancies
            For<IApprenticeshipSummaryUpdateProcessor>().Use<ApprenticeshipSummaryUpdateProcessor>();
            For<ITraineeshipsSummaryUpdateProcessor>().Use<TraineeshipsSummaryUpdateProcessor>();

            For<ApprenticeshipSummaryUpdateProcessor>().Use<ApprenticeshipSummaryUpdateProcessor>();
            For<TraineeshipsSummaryUpdateProcessor>().Use<TraineeshipsSummaryUpdateProcessor>();

            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";//todo: remove
            For<IVacancySummaryProcessor>().Use<VacancySummaryProcessor>().Ctor<IMapper>().Named("VacancyEtlMapper");

            For<IVacancyStatusProcessor>().Use<VacancyStatusProcessor>();

            // site map
            For<ISiteMapVacancyProcessor>().Use<SiteMapVacancyProcessor>();
            For<ISiteMapVacancyProvider>().Use<SiteMapVacancyProvider>();

            // reference data
            For<IReferenceDataService>().Use<ReferenceDataService>();

            // application housekeeping
            For<IRootApplicationHousekeeper>().Use<RootApplicationHousekeeper>();
            For<IDraftApplicationForExpiredVacancyHousekeeper>().Use<DraftApplicationForExpiredVacancyHousekeeper>();
            For<ISubmittedApplicationHousekeeper>().Use<SubmittedApplicationHousekeeper>();
            For<IHardDeleteApplicationStrategy>().Use<HardDeleteApplicationStrategy>();
            For<IAuditRepository>().Use<AuditRepository>();

            // communication housekeeping
            For<IRootCommunicationHousekeeper>().Use<RootCommunicationHousekeeper>();
            For<IApplicationStatusAlertCommunicationHousekeeper>().Use<ApplicationStatusAlertCommunicationHousekeeper>();
            For<IExpiringDraftApplicationAlertCommunicationHousekeeper>().Use<ExpiringDraftApplicationAlertCommunicationHousekeeper>();
            For<ISavedSearchAlertCommunicationHousekeeper>().Use<SavedSearchAlertCommunicationHousekeeper>();

            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ISavedSearchProcessor>().Use<SavedSearchProcessor>();

            For<IProviderService>().Use<ProviderService>();
            For<IOrganisationService>().Use<OrganisationService>();
            For<IEmployerService>().Use<EmployerService>();

            // service bus
            RegisterServiceBusMessageBrokers(container);

            RegisterStrategies();
        }

        #region Helpers

        private void RegisterServiceBusMessageBrokers(Container container)
        {
            RegisterVacancyAboutToExpireServiceBusMessageBroker();
            RegisterCommunicationServiceBusMessageBrokers(container);

            RegisterServiceBusMessageBroker<ApplicationStatusSummarySubscriber, ApplicationStatusSummary>();
            RegisterServiceBusMessageBroker<VacancyStatusSummarySubscriber, VacancyStatusSummary>();
            RegisterServiceBusMessageBroker<ApplicationHousekeepingRequestSubscriber, ApplicationHousekeepingRequest>();
            RegisterServiceBusMessageBroker<ApplicationStatusChangedSubscriber, ApplicationStatusChanged>();
            RegisterServiceBusMessageBroker<ApplicationStatusSummaryPageSubscriber, ApplicationUpdatePage>();
            RegisterServiceBusMessageBroker<CommunicationHousekeepingRequestSubscriber, CommunicationHousekeepingRequest>();
            RegisterServiceBusMessageBroker<SubmitApprenticeshipApplicationRequestSubscriber, SubmitApprenticeshipApplicationRequest>();
            RegisterServiceBusMessageBroker<SubmitTraineeshipApplicationRequestSubscriber, SubmitTraineeshipApplicationRequest>();
            RegisterServiceBusMessageBroker<CandidateAccountHousekeepingSubscriber, CandidateHousekeeping>();
            RegisterServiceBusMessageBroker<CandidateSavedSearchesSubscriber, CandidateSavedSearches>();
            RegisterServiceBusMessageBroker<CreateCandidateRequestSubscriber, CreateCandidateRequest>();
            RegisterServiceBusMessageBroker<SaveCandidateRequestSubscriber, SaveCandidateRequest>();
            RegisterServiceBusMessageBroker<CommunicationRequestSubscriber, CommunicationRequest>();
            RegisterServiceBusMessageBroker<CreateVacancySiteMapRequestSubscriber, CreateVacancySiteMapRequest>();
            RegisterServiceBusMessageBroker<VacancyStatusSummarySubscriber, VacancyStatusSummary>();
            RegisterServiceBusMessageBroker<VacancyStatusProcessorSubscriber, VacancyEligibleForClosure>();
            RegisterServiceBusMessageBroker<VacancySummaryCompleteSubscriber, VacancySummaryUpdateComplete>();
        }

        private void RegisterCommunicationServiceBusMessageBrokers(Container container)
        {
            // Plug-in configured email, SMS etc. providers.
            var configurationService = container.GetInstance<IConfigurationService>();
            var communicationConfiguration = configurationService.Get<CommunicationConfiguration>();

            For<IServiceBusSubscriber<EmailRequest>>()
                .Use<EmailRequestSubscriber>()
                .Ctor<IEmailDispatcher>()
                .Named(communicationConfiguration.EmailDispatcher);
            For<IServiceBusMessageBroker>().Use<AzureServiceBusMessageBroker<EmailRequest>>();

            For<IServiceBusSubscriber<SmsRequest>>()
                .Use<SmsRequestSubscriber>()
                .Ctor<ISmsDispatcher>()
                .Named(communicationConfiguration.SmsDispatcher);
            For<IServiceBusMessageBroker>().Use<AzureServiceBusMessageBroker<SmsRequest>>();
        }

        private void RegisterVacancyAboutToExpireServiceBusMessageBroker()
        {
            // TODO: AG: this mapper is slated for removal (see TODO above).
            For<IServiceBusSubscriber<VacancyAboutToExpire>>()
                .Use<VacancyAboutToExpireSubscriber>()
                .Ctor<IMapper>()
                .Named("VacancyEtlMapper");

            For<IServiceBusMessageBroker>().Use<AzureServiceBusMessageBroker<VacancyAboutToExpire>>();
        }

        private void RegisterServiceBusMessageBroker<TSubscriber, TMessage>()
            where TSubscriber : IServiceBusSubscriber<TMessage>
            where TMessage : class
        {
            For<IServiceBusSubscriber<TMessage>>().Use<TSubscriber>();
            For<IServiceBusMessageBroker>().Use<AzureServiceBusMessageBroker<TMessage>>();
        }

        private void RegisterStrategies()
        {
            For<IGetByIdStrategy>().Use<GetByIdStrategy>();
            For<IGetByIdWithoutStatusCheckStrategy>().Use<GetByIdWithoutStatusCheckStrategy>();
            For<IGetByIdsStrategy>().Use<GetByIdsStrategy>();
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<IGetPagedEmployerSearchResultsStrategy>().Use<GetPagedEmployerSearchResultsStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<ISaveEmployerStrategy>().Use<SaveEmployerStrategy>();
            For<ISendEmployerLinksStrategy>().Use<SendEmployerLinksStrategy>();
        }

        #endregion
    }
}