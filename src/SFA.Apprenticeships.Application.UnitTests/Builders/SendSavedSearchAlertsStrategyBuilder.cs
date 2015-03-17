namespace SFA.Apprenticeships.Application.UnitTests.Builders
{
    using Application.Communications.Strategies;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Moq;

    public class SendSavedSearchAlertsStrategyBuilder
    {
        private Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();
        private Mock<ISavedSearchAlertRepository> _savedSearchAlertRepository = new Mock<ISavedSearchAlertRepository>();
        private Mock<IMessageBus> _messageBus = new Mock<IMessageBus>();

        public SendSavedSearchAlertsStrategyBuilder With(Mock<ICandidateReadRepository> candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            return this;
        }

        public SendSavedSearchAlertsStrategyBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }

        public SendSavedSearchAlertsStrategyBuilder With(Mock<ISavedSearchAlertRepository> savedSearchAlertRepository)
        {
            _savedSearchAlertRepository = savedSearchAlertRepository;
            return this;
        }

        public SendSavedSearchAlertsStrategyBuilder With(Mock<IMessageBus> messageBus)
        {
            _messageBus = messageBus;
            return this;
        }

        public SendSavedSearchAlertsStrategy Build()
        {
            return new SendSavedSearchAlertsStrategy(
                _savedSearchAlertRepository.Object, _candidateReadRepository.Object, _userReadRepository.Object, _messageBus.Object);
        }
    }
}
