namespace SFA.Apprenticeships.Infrastructure.Processes.IoC
{
    using Application.Applications;
    using Application.Applications.Strategies;
    using Application.Candidates.Strategies;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Logging;
    using Application.Interfaces.ReferenceData;
    using Application.Location;
    using Application.ReferenceData;
    using Application.Vacancies;
    using Application.Vacancy.SiteMap;
    using Applications;
    using Candidates;
    using Common.IoC;
    using Communication.Configuration;
    using Communications;
    using Communications.Commands;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Logging.IoC;
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

            var configService = container.GetInstance<IConfigurationService>();
            var commsConfig = configService.Get<CommunicationConfiguration>();

            For<EmailRequestConsumerAsync>().Use<EmailRequestConsumerAsync>().Ctor<IEmailDispatcher>().Named(commsConfig.EmailDispatcher);
            For<SmsRequestConsumerAsync>().Use<SmsRequestConsumerAsync>().Ctor<ISmsDispatcher>().Named(commsConfig.SmsDispatcher);

            For<CommunicationCommand>().Use<CandidateCommunicationCommand>();
            For<CommunicationCommand>().Use<CandidateDailyDigestCommunicationCommand>();
            For<CommunicationCommand>().Use<HelpDeskCommunicationCommand>();

            For<CommunicationRequestConsumerAsync>().Use<CommunicationRequestConsumerAsync>();

            // applications
            For<SubmitApprenticeshipApplicationRequestConsumerAsync>().Use<SubmitApprenticeshipApplicationRequestConsumerAsync>();
            For<SubmitTraineeshipApplicationRequestConsumerAsync>().Use<SubmitTraineeshipApplicationRequestConsumerAsync>();
            For<ApplicationStatusChangedConsumerAsync>().Use<ApplicationStatusChangedConsumerAsync>();
            For<IApplicationStatusProcessor>().Use<ApplicationStatusProcessor>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();

            // vacancies
            For<IApprenticeshipSummaryUpdateProcessor>().Use<ApprenticeshipSummaryUpdateProcessor>();
            For<ITraineeshipsSummaryUpdateProcessor>().Use<TraineeshipsSummaryUpdateProcessor>();

            For<VacancyStatusSummaryConsumerAsync>().Use<VacancyStatusSummaryConsumerAsync>();
            For<ApprenticeshipSummaryUpdateProcessor>().Use<ApprenticeshipSummaryUpdateProcessor>();
            For<TraineeshipsSummaryUpdateProcessor>().Use<TraineeshipsSummaryUpdateProcessor>();

            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";//todo: remove
            For<IVacancySummaryProcessor>().Use<VacancySummaryProcessor>().Ctor<IMapper>().Named("VacancyEtlMapper");
            For<VacancyAboutToExpireConsumerAsync>().Use<VacancyAboutToExpireConsumerAsync>().Ctor<IMapper>().Named("VacancyEtlMapper");

            // site map
            For<ISiteMapVacancyProcessor>().Use<SiteMapVacancyProcessor>();
            For<ISiteMapVacancyProvider>().Use<SiteMapVacancyProvider>();

            // reference data
            For<IReferenceDataService>().Use<ReferenceDataService>();

            // candidates
            For<CandidateSavedSearchesConsumerAsync>().Use<CandidateSavedSearchesConsumerAsync>();
            For<CreateCandidateRequestConsumerAsync>().Use<CreateCandidateRequestConsumerAsync>();
            For<CandidateAccountHousekeepingConsumerAsync>().Use<CandidateAccountHousekeepingConsumerAsync>().Ctor<IHousekeepingStrategy>().Is(context => BuildHousekeepingChainOfResponsibility(context));

            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ISavedSearchProcessor>().Use<SavedSearchProcessor>();
        }

        private static IHousekeepingStrategy BuildHousekeepingChainOfResponsibility(IContext context)
        {
            var configurationService = context.GetInstance<IConfigurationService>();
            var logService = context.GetInstance<ILogService>();

            var sendAccountRemindersStrategy = new SendAccountRemindersStrategy(configurationService, logService);
            var setPendingDeletionStrategy = new SetPendingDeletionStrategy(configurationService, logService);

            sendAccountRemindersStrategy.SetSuccessor(setPendingDeletionStrategy);
            setPendingDeletionStrategy.SetSuccessor(new TerminatingHousekeepingStrategy(configurationService));

            return sendAccountRemindersStrategy;
        }
    }
}
