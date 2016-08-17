namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using Application.Interfaces.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Moq;

    using SFA.Apprenticeships.Application.Interfaces;

    public class TraineeshipApplicationProviderBuilder
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        private Mock<ICandidateService> _candidateService;
        private Mock<ITraineeshipVacancyProvider> _traineeshipVacancyProvider;

        public TraineeshipApplicationProviderBuilder()
        {
            _mapper = new TraineeshipCandidateWebMappers();
            _logger = new Mock<ILogService>().Object;

            _candidateService = new Mock<ICandidateService>();
            _traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
        }

        public TraineeshipApplicationProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            _candidateService = candidateService;
            return this;
        }

        public TraineeshipApplicationProviderBuilder With(Mock<ITraineeshipVacancyProvider> traineeshipVacancyProvider)
        {
            _traineeshipVacancyProvider = traineeshipVacancyProvider;
            return this;
        }

        public TraineeshipApplicationProvider Build()
        {
            var provider = new TraineeshipApplicationProvider(_mapper, _candidateService.Object, _traineeshipVacancyProvider.Object, _logger);
            return provider;
        }
    }
}