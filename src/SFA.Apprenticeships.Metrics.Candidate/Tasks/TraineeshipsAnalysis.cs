namespace SFA.Apprenticeships.Metrics.Candidate.Tasks
{
    using Infrastructure.Monitor.Repositories;

    public class TraineeshipsAnalysis : IMetricsTask
    {
        private readonly IApprenticeshipMetricsRepository _apprenticeshipMetricsRepository;

        public TraineeshipsAnalysis(IApprenticeshipMetricsRepository apprenticeshipMetricsRepository)
        {
            _apprenticeshipMetricsRepository = apprenticeshipMetricsRepository;
        }

        public string TaskName
        {
            get { return "Traineeships Analysis"; }
        }

        public void Run()
        {
            var results = _apprenticeshipMetricsRepository.GetApplicationStatusCounts();
        }
    }
}