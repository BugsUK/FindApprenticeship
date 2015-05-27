using Moq;
using SFA.Apprenticeships.Application.Candidates.Strategies;
using SFA.Apprenticeships.Application.Interfaces.Logging;
using SFA.Apprenticeships.Domain.Interfaces.Configuration;
using SFA.Apprenticeships.Domain.Interfaces.Repositories;

namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using Application.Candidates.Configuration;
    using Configuration;

    public class HardDeleteStrategyBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private readonly Mock<IUserWriteRepository> _userWriteRepository = new Mock<IUserWriteRepository>();
        private readonly Mock<ICandidateWriteRepository> _candidateWriteRepository = new Mock<ICandidateWriteRepository>();
        private readonly Mock<IAuditRepository> _auditRepository = new Mock<IAuditRepository>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private IHousekeepingStrategy _successor;

        public HardDeleteStrategyBuilder()
        {
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
            _successor = new TerminatingHousekeepingStrategy(_configurationService.Object);
        }

        public HardDeleteStrategy Build()
        {
            var strategy = new HardDeleteStrategy(_configurationService.Object, _userWriteRepository.Object, _candidateWriteRepository.Object, _auditRepository.Object, _logService.Object);
            strategy.SetSuccessor(_successor);
            return strategy;
        }

        public HardDeleteStrategyBuilder With(IHousekeepingStrategy successor)
        {
            _successor = successor;
            return this;
        }
    }
}