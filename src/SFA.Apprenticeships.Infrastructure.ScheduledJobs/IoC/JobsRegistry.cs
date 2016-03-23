namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.IoC
{
    using SFA.Infrastructure.Interfaces;
    using Application.Applications;
    using Application.Applications.Housekeeping;
    using Application.Applications.Strategies;
    using Application.Candidates;
    using Application.Communications;
    using Application.Communications.Housekeeping;
    using Application.Communications.Strategies;
    using Application.Employer;
    using Application.Employer.Strategies;
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
    using Consumers;
    using Domain.Interfaces.Repositories;
    using Processes.Vacancies;
    using Repositories.Mongo.Audit;
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
            For<IProviderService>().Use<ProviderService>();
            For<IOrganisationService>().Use<OrganisationService>();
            For<IEmployerService>().Use<EmployerService>();

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

            RegisterStrategies();
        }

        private void RegisterStrategies()
        {
            For<IGetByIdStrategy>().Use<GetByIdStrategy>();
            For<IGetByIdsStrategy>().Use<GetByIdsStrategy>();
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<IGetPagedEmployerSearchResultsStrategy>().Use<GetPagedEmployerSearchResultsStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<ISaveEmployerStrategy>().Use<SaveEmployerStrategy>();
        }
    }
}