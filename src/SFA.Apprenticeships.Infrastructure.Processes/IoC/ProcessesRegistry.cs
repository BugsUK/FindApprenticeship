namespace SFA.Apprenticeships.Infrastructure.Processes.IoC
{
    using Application.Applications;
    using Application.Applications.Strategies;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Locations;
    using Application.Interfaces.ReferenceData;
    using Application.Location;
    using Application.ReferenceData;
    using Application.Vacancies;
    using Applications;
    using Candidates;
    using Common.IoC;
    using Communication.Configuration;
    using Communications;
    using Communications.Commands;
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

            var commsConfig = container.GetInstance<CommunicationConfiguration>(CommunicationConfiguration.ConfigurationName);

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
            For<VacancyStatusSummaryConsumerAsync>().Use<VacancyStatusSummaryConsumerAsync>();
            For<ApprenticeshipSummaryUpdateConsumerAsync>().Use<ApprenticeshipSummaryUpdateConsumerAsync>();
            For<TraineeshipsSummaryUpdateConsumerAsync>().Use<TraineeshipsSummaryUpdateConsumerAsync>();

            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";//todo: remove
            For<IVacancySummaryProcessor>().Use<VacancySummaryProcessor>().Ctor<IMapper>().Named("VacancyEtlMapper");
            For<VacancyAboutToExpireConsumerAsync>().Use<VacancyAboutToExpireConsumerAsync>().Ctor<IMapper>().Named("VacancyEtlMapper");

            For<IReferenceDataService>().Use<ReferenceDataService>();

            // candidates
            For<CandidateSavedSearchesConsumerAsync>().Use<CandidateSavedSearchesConsumerAsync>();
            For<CreateCandidateRequestConsumerAsync>().Use<CreateCandidateRequestConsumerAsync>();

            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ISavedSearchProcessor>().Use<SavedSearchProcessor>();
        }
    }
}
