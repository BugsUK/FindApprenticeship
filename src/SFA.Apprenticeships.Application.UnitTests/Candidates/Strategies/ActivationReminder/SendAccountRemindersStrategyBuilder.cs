namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.ActivationReminder
{
    using Application.Candidates.Configuration;
    using Application.Candidates.Strategies;
    using Application.Candidates.Strategies.ActivationReminder;
    using Configuration;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Communications;
    using Moq;

    public class SendAccountRemindersStrategyBuilder
    {
        private Mock<IConfigurationService> _configurationService;
        private Mock<ICommunicationService> _communicationService;

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

        public SendAccountRemindersStrategyBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }

        public SendAccountRemindersStrategyBuilder With(HousekeepingConfiguration configuration)
        {
            _configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(configuration);
            return this;
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