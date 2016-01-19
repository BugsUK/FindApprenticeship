using Moq;
using SFA.Apprenticeships.Application.Candidates.Strategies;
using SFA.Infrastructure.Interfaces;
using SFA.Apprenticeships.Domain.Interfaces.Repositories;

namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using Application.Candidates.Configuration;
    using Configuration;

    public class HardDeleteStrategyBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private Mock<IUserWriteRepository> _userWriteRepository = new Mock<IUserWriteRepository>();
        private Mock<IAuthenticationRepository> _authenticationRepository = new Mock<IAuthenticationRepository>();
        private Mock<ICandidateWriteRepository> _candidateWriteRepository = new Mock<ICandidateWriteRepository>();
        private Mock<ISavedSearchReadRepository> _savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
        private Mock<ISavedSearchWriteRepository> _savedSearchWriteRepository = new Mock<ISavedSearchWriteRepository>();
        private Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
        private Mock<IApprenticeshipApplicationWriteRepository> _apprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
        private Mock<ITraineeshipApplicationReadRepository> _traineeshipApplicationReadRepository = new Mock<ITraineeshipApplicationReadRepository>();
        private Mock<ITraineeshipApplicationWriteRepository> _traineeshipApplicationWriteRepository = new Mock<ITraineeshipApplicationWriteRepository>();
        private Mock<IAuditRepository> _auditRepository = new Mock<IAuditRepository>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private IHousekeepingStrategy _successor;

        public HardDeleteStrategyBuilder()
        {
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
            _successor = new TerminatingHousekeepingStrategy(_configurationService.Object);
        }

        public HardDeleteStrategy Build()
        {
            var strategy = new HardDeleteStrategy(_configurationService.Object, _userWriteRepository.Object, _authenticationRepository.Object,
                _candidateWriteRepository.Object, _savedSearchReadRepository.Object, _savedSearchWriteRepository.Object, _apprenticeshipApplicationReadRepository.Object,
                _apprenticeshipApplicationWriteRepository.Object, _traineeshipApplicationReadRepository.Object,
                _traineeshipApplicationWriteRepository.Object, _auditRepository.Object, _logService.Object);
            strategy.SetSuccessor(_successor);
            return strategy;
        }

        public HardDeleteStrategyBuilder With(IHousekeepingStrategy successor)
        {
            _successor = successor;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<IApprenticeshipApplicationReadRepository> apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<ITraineeshipApplicationReadRepository> traineeshipApplicationReadRepository)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<IUserWriteRepository> userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<ICandidateWriteRepository> candidateWriteRepository)
        {
            _candidateWriteRepository = candidateWriteRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<IApprenticeshipApplicationWriteRepository> apprenticeshipApplicationWriteRepository)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<ITraineeshipApplicationWriteRepository> traineeshipApplicationWriteRepository)
        {
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<IAuditRepository> auditRepository)
        {
            _auditRepository = auditRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<ISavedSearchReadRepository> savedSearchReadRepository)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<ISavedSearchWriteRepository> savedSearchWriteRepository)
        {
            _savedSearchWriteRepository = savedSearchWriteRepository;
            return this;
        }

        public HardDeleteStrategyBuilder With(Mock<IAuthenticationRepository> authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
            return this;
        }
    }
}