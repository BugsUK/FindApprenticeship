namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.Traineeships
{
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Moq;

    public class TraineeshipApplicationProviderBuilder
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        private Mock<ICandidateService> _candidateService;
        private Mock<ITraineeshipVacancyProvider> _traineeshipVacancyProvider;
        private Mock<IConfigurationService> _configurationService;

        public TraineeshipApplicationProviderBuilder()
        {
            _mapper = new TraineeshipCandidateWebMappers();
            _logger = new Mock<ILogService>().Object;

            _candidateService = new Mock<ICandidateService>();
            _configurationService = new Mock<IConfigurationService>();
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

        public TraineeshipApplicationProviderBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }

        public TraineeshipApplicationProvider Build()
        {
            var provider = new TraineeshipApplicationProvider(_mapper, _candidateService.Object, _traineeshipVacancyProvider.Object, _logger, _configurationService.Object);
            return provider;
        }
    }
}