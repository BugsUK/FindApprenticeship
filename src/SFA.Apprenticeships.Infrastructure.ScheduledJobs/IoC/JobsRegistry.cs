namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.IoC
{
    using Application.Applications;
    using Application.Applications.Housekeeping;
    using Application.Applications.Strategies;
    using Application.Candidates;
    using Application.Communications;
    using Application.Communications.Housekeeping;
    using Application.Communications.Strategies;
    using Application.Interfaces.Locations;
    using Application.Interfaces.ReferenceData;
    using Application.Location;
    using Application.ReferenceData;
    using Application.Vacancies;
    using Consumers;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Processes.Vacancies;
    using Repositories.Audit;
    using StructureMap.Configuration.DSL;
    using VacancyEtlMapper = Mappers.VacancyEtlMapper;

    public class JobsRegistry : Registry
    {
        public JobsRegistry()
        {
            //Application Etl
            For<ApplicationEtlControlQueueConsumer>().Use<ApplicationEtlControlQueueConsumer>();
            For<IApplicationStatusProcessor>().Use<ApplicationStatusProcessor>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();

            //Vacancy Etl
            For<VacancyEtlControlQueueConsumer>().Use<VacancyEtlControlQueueConsumer>();
            For<SavedSearchControlQueueConsumer>().Use<SavedSearchControlQueueConsumer>();

            For<IReferenceDataService>().Use<ReferenceDataService>();

            For<IApprenticeshipSummaryUpdateProcessor>().Use<ApprenticeshipSummaryUpdateProcessor>();
            For<ITraineeshipsSummaryUpdateProcessor>().Use<TraineeshipsSummaryUpdateProcessor>();

            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";
            For<IVacancySummaryProcessor>().Use<VacancySummaryProcessor>().Ctor<IMapper>().Named("VacancyEtlMapper");

            For<IVacancyStatusProcessor>().Use<VacancyStatusProcessor>();

            //Communications
            For<DailyDigestControlQueueConsumer>().Use<DailyDigestControlQueueConsumer>();
            For<ICommunicationProcessor>().Use<CommunicationProcessor>();
            For<ISendDailyDigestsStrategy>().Use<SendDailyDigestsStrategy>();
            For<ISendSavedSearchAlertsStrategy>().Use<SendSavedSearchAlertsStrategy>();

            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ISavedSearchProcessor>().Use<SavedSearchProcessor>();

            //Candidate Housekeeping
            For<ICandidateProcessor>().Use<CandidateProcessor>();
            
            // Application Housekeeping
            For<IRootApplicationHousekeeper>().Use<RootApplicationHousekeeper>();
            For<IDraftApplicationForExpiredVacancyHousekeeper>().Use<DraftApplicationForExpiredVacancyHousekeeper>();
            For<ISubmittedApplicationHousekeeper>().Use<SubmittedApplicationHousekeeper>();
            For<IHardDeleteApplicationStrategy>().Use<HardDeleteApplicationStrategy>();
            For<IAuditRepository>().Use<AuditRepository>();

            // Communication Housekeeping
            For<IRootCommunicationHousekeeper>().Use<RootCommunicationHousekeeper>();
            For<IApplicationStatusAlertCommunicationHousekeeper>().Use<ApplicationStatusAlertCommunicationHousekeeper>();
            For<IExpiringDraftApplicationAlertCommunicationHousekeeper>().Use<ExpiringDraftApplicationAlertCommunicationHousekeeper>();
            For<ISavedSearchAlertCommunicationHousekeeper>().Use<SavedSearchAlertCommunicationHousekeeper>();

            For<HousekeepingControlQueueConsumer>().Use<HousekeepingControlQueueConsumer>();

            // Vacancy Housekeeping
            For<VacancyStatusControlQueueConsumer>().Use<VacancyStatusControlQueueConsumer>();
        }
    }
}