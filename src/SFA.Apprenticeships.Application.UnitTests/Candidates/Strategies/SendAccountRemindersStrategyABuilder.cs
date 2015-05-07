namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using Application.Candidates.Configuration;
    using Application.Candidates.Strategies;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Interfaces.Logging;
    using Moq;

    public class SendAccountRemindersStrategyABuilder
    {
        private Mock<IConfigurationService> _configurationService;
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private IHousekeepingStrategy _successor; 

        public SendAccountRemindersStrategyABuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
            _successor = new TerminatingHousekeepingStrategy(_configurationService.Object);
        }

        public SendAccountRemindersStrategyA Build()
        {
            var strategy = new SendAccountRemindersStrategyA(_configurationService.Object, _logService.Object);
            strategy.SetSuccessor(_successor);
            return strategy;
        }

        public SendAccountRemindersStrategyABuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }

        public SendAccountRemindersStrategyABuilder With(HousekeepingConfiguration configuration)
        {
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(configuration);
            return this;
        }

        public SendAccountRemindersStrategyABuilder With(IHousekeepingStrategy successor)
        {
            _successor = successor;
            return this;
        }
    }
}