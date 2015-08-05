namespace SFA.Apprenticeships.Metrics.Candidate.IoC
{
    using Infrastructure.Monitor.Repositories;
    using StructureMap.Configuration.DSL;
    using Tasks;

    public class MetricsRepository : Registry
    {
        public MetricsRepository()
        {
            For<IUserMetricsRepository>().Use<UserMetricsRepository>();
            For<ICandidateMetricsRepository>().Use<CandidateMetricsRepository>();
            For<IApprenticeshipMetricsRepository>().Use<ApprenticeshipMetricsRepository>();
            For<ITraineeshipMetricsRepository>().Use<TraineeshipMetricsRepository>();

            For<IMetricsTaskRunner>().Use<MetricsTaskRunner>()
                .EnumerableOf<IMetricsTask>()
                .Contains(x =>
                {
                    x.Type<TraineeshipsAnalysis>();
                    x.Type<EShot>();
                });
        }
    }
}