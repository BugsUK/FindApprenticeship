namespace SFA.Apprenticeships.Infrastructure.Monitor.IoC
{
    using Application.Vacancies;
    using Consumers;
    using Domain.Interfaces.Mapping;
    using LegacyWebServices.Vacancy;
    using Mongo.Common;
    using Provider;
    using Repositories;
    using StructureMap.Configuration.DSL;
    using Tasks;

    public class MonitorRegistry : Registry
    {
        public MonitorRegistry()
        {
            For<MonitorControlQueueConsumer>().Use<MonitorControlQueueConsumer>();
            For<IMonitorTasksRunner>().Use<MonitorTasksRunner>();

            //TODO: remove once link with AV is broken
            For<IVacancyIndexDataProvider>().Use<LegacyVacancyIndexDataProvider>().Ctor<IMapper>().Named("LegacyWebServices.LegacyVacancySummaryMapper");

            For<IMonitorTasksRunner>().Use<MonitorTasksRunner>()
                .EnumerableOf<IMonitorTask>()
                .Contains(x =>
                {
                    x.Type<CheckUserRepository>();
                    x.Type<CheckApprenticeshipApplicationRepository>();
                    x.Type<CheckTraineeshipApplicationRepository>();
                    x.Type<CheckCandidateRepository>();
                    x.Type<CheckVacancySearch>();
                    x.Type<CheckLocationLookup>();
                    x.Type<CheckPostcodeService>();
                    x.Type<CheckUserDirectory>();
                    x.Type<CheckAzureServiceBus>();
                    //TODO: remove once link with AV is broken
                    x.Type<CheckNasGateway>();
                    x.Type<CheckMongoReplicaSets>();
                    x.Type<CheckElasticsearchCluster>();
                    x.Type<CheckElasticsearchAliases>();
                    x.Type<CheckLogstashLogs>();
                    //x.Type<CheckUnsentCandidateMessages>();
                });

            For<IDailyMetricsTasksRunner>().Use<DailyMetricsTasksRunner>()
                .EnumerableOf<IDailyMetricsTask>()
                .Contains(x => x.Type<SendDailyMetricsEmail>());

            For<IMongoAdminClient>().Use<MongoAdminClient>();
            For<IApprenticeshipMetricsRepository>().Use<ApprenticeshipMetricsRepository>();
            For<IExpiringDraftsMetricsRepository>().Use<ExpiringDraftsMetricsRepository>();
            For<IApplicationStatusAlertsMetricsRepository>().Use<ApplicationStatusAlertsMetricsRepository>();
            For<ITraineeshipMetricsRepository>().Use<TraineeshipMetricsRepository>();
            For<IUserMetricsRepository>().Use<UserMetricsRepository>();
            For<ICandidateDiagnosticsRepository>().Use<CandidateDiagnosticsRepository>();
            For<ISavedSearchAlertMetricsRepository>().Use<SavedSearchAlertMetricsRepository>();
            For<IContactMessagesMetricsRepository>().Use<ContactMessagesMetricsRepository>();
            For<ISavedSearchesMetricsRepository>().Use<SavedSearchesMetricsRepository>();
            For<ICandidateMetricsRepository>().Use<CandidateMetricsRepository>();
            For<IAuditMetricsRepository>().Use<AuditMetricsRepository>();

            For<IVacancyMetricsProvider>().Use<VacancyMetricsProvider>();
        }
    }
}