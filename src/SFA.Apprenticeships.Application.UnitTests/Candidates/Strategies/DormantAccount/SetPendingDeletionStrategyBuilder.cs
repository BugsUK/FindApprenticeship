namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.DormantAccount
{
    using Application.Candidates.Configuration;
    using Application.Candidates.Strategies;
    using Application.Candidates.Strategies.DormantAccount;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Moq;

    public class SetPendingDeletionStrategyBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService;
        private Mock<IUserWriteRepository> _userWriteRepository;
        private readonly Mock<IAuditRepository> _auditRepository = new Mock<IAuditRepository>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private IHousekeepingStrategy _successor;

        public SetPendingDeletionStrategyBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
            _userWriteRepository = new Mock<IUserWriteRepository>();
            _successor = new TerminatingHousekeepingStrategy(_configurationService.Object);
        }

        public SetPendingDeletionStrategy Build()
        {
            var strategy = new SetPendingDeletionStrategy(_configurationService.Object, _userWriteRepository.Object, _auditRepository.Object, _logService.Object);
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
    }
}