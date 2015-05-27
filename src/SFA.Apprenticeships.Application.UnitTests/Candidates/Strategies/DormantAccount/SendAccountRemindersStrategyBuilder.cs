namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.DormantAccount
{
    using Application.Candidates.Configuration;
    using Application.Candidates.Strategies;
    using Application.Candidates.Strategies.DormantAccount;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Interfaces.Communications;
    using Interfaces.Logging;
    using Moq;

    public class SendAccountRemindersStrategyBuilder
    {
        private Mock<IConfigurationService> _configurationService;
        private Mock<ICommunicationService> _communicationService;
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private IHousekeepingStrategy _successor;

        public SendAccountRemindersStrategyBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().Build());
            _communicationService = new Mock<ICommunicationService>();
            _successor = new TerminatingHousekeepingStrategy(_configurationService.Object);
        }

        public SendAccountRemindersStrategy Build()
        {
            var strategy = new SendAccountRemindersStrategy(_configurationService.Object, _communicationService.Object);
            strategy.SetSuccessor(_successor);
            return strategy;
        }

        public SendAccountRemindersStrategyBuilder With(IHousekeepingStrategy successor)
        {
            _successor = successor;
            return this;
        }

        public SendAccountRemindersStrategyBuilder With(Mock<ICommunicationService> communicationService)
        {
            _communicationService = communicationService;
            return this;
        }
    }
}