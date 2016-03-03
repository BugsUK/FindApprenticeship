namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.DormantAccount
{
    using Apprenticeships.Application.Candidates.Configuration;
    using Apprenticeships.Application.Candidates.Strategies;
    using Apprenticeships.Application.Candidates.Strategies.DormantAccount;
    using Configuration;
    using Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Moq;

    public class SetPendingDeletionStrategyBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService;
        private Mock<IUserReadRepository> _userReadRepository;
        private Mock<IUserWriteRepository> _userWriteRepository;
        private readonly Mock<IAuditRepository> _auditRepository = new Mock<IAuditRepository>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private IHousekeepingStrategy _successor;

        public SetPendingDeletionStrategyBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
            _userReadRepository = new Mock<IUserReadRepository>();
            _userWriteRepository = new Mock<IUserWriteRepository>();
            _successor = new TerminatingHousekeepingStrategy(_configurationService.Object);
        }

        public SetPendingDeletionStrategy Build()
        {
            var strategy = new SetPendingDeletionStrategy(_configurationService.Object, _userReadRepository.Object, _userWriteRepository.Object, _auditRepository.Object, _logService.Object);
            strategy.SetSuccessor(_successor);
            return strategy;
        }

        public SetPendingDeletionStrategyBuilder With(IHousekeepingStrategy successor)
        {
            _successor = successor;
            return this;
        }

        public SetPendingDeletionStrategyBuilder With(Mock<IUserWriteRepository> userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
            return this;
        }

        public SetPendingDeletionStrategyBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }
    }
}