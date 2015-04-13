namespace SFA.Apprenticeships.Metrics.Candidate.Tasks
{
    using Infrastructure.Monitor.Repositories;

    public class TraineeshipsAnalysis : IMetricsTask
    {
        private readonly IUserMetricsRepository _userMetricsRepository;
        private readonly ICandidateMetricsRepository _candidateMetricsRepository;
        private readonly IApprenticeshipMetricsRepository _apprenticeshipMetricsRepository;
        private readonly ITraineeshipMetricsRepository _traineeshipMetricsRepository;

        public TraineeshipsAnalysis(IUserMetricsRepository userMetricsRepository, ICandidateMetricsRepository candidateMetricsRepository, IApprenticeshipMetricsRepository apprenticeshipMetricsRepository, ITraineeshipMetricsRepository traineeshipMetricsRepository)
        {
            _userMetricsRepository = userMetricsRepository;
            _candidateMetricsRepository = candidateMetricsRepository;
            _apprenticeshipMetricsRepository = apprenticeshipMetricsRepository;
            _traineeshipMetricsRepository = traineeshipMetricsRepository;
        }

        public string TaskName
        {
            get { return "Traineeships Analysis"; }
        }

        public void Run()
        {
            var userActivityMetrics = _userMetricsRepository.GetUserActivityMetrics();
            var apprenticeshipApplicationsStatusCounts = _apprenticeshipMetricsRepository.GetApplicationStatusCounts();
            var traineeshipApplicationsStatusCounts = _traineeshipMetricsRepository.GetApplicationStatusCounts();
            var candidatesThatWouldHaveSeenTraineeshipPrompt = _apprenticeshipMetricsRepository.GetCandidatesThatWouldHaveSeenTraineeshipPrompt();
            var candidatesThatHaveDismissedTheTraineeshipPrompt = _candidateMetricsRepository.GetCandidatesThatHaveDismissedTheTraineeshipPrompt();

            var applicationCountPerApprenticeship = _apprenticeshipMetricsRepository.GetApplicationCountPerApprenticeship();
            var averageApplicationCountPerApprenticeship = _apprenticeshipMetricsRepository.GetAverageApplicationCountPerApprenticeship();

            var applicationCountPerTraineeship = _traineeshipMetricsRepository.GetApplicationCountPerTraineeship();
            var averageApplicationCountPerTraineeship = _traineeshipMetricsRepository.GetAverageApplicationCountPerTraineeship();
        }
    }
}