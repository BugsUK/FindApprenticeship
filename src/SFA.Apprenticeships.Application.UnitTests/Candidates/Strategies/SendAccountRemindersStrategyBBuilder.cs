namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using Application.Candidates.Configuration;
    using Application.Candidates.Strategies;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Interfaces.Communications;
    using Interfaces.Logging;
    using Moq;

    public class SendAccountRemindersStrategyBBuilder
    {
        private Mock<IConfigurationService> _configurationService;
        private Mock<ICommunicationService> _communicationService;
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private IHousekeepingStrategy _successor; 

        public SendAccountRemindersStrategyBBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
            _communicationService = new Mock<ICommunicationService>();
            _successor = new TerminatingHousekeepingStrategy(_configurationService.Object);
        }

        public SendAccountRemindersStrategyB Build()
        {
            var strategy = new SendAccountRemindersStrategyB(_configurationService.Object, _communicationService.Object, _logService.Object);
            strategy.SetSuccessor(_successor);
            return strategy;
        }

        public SendAccountRemindersStrategyBBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }

        public SendAccountRemindersStrategyBBuilder With(HousekeepingConfiguration configuration)
        {
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(configuration);
            return this;
        }

        public SendAccountRemindersStrategyBBuilder With(IHousekeepingStrategy successor)
        {
            _successor = successor;
            return this;
        }

        public SendAccountRemindersStrategyBBuilder With(Mock<ICommunicationService> communicationService)
        {
            _communicationService = communicationService;
            return this;
        }
    }
}