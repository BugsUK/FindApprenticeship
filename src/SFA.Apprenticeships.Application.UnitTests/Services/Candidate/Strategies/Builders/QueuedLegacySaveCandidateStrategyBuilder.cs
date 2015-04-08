namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies.Builders
{
    using Application.Candidate.Strategies;
    using Domain.Interfaces.Messaging;
    using Domain.Entities.Candidates;
    using Moq;

    public class QueuedLegacySaveCandidateStrategyBuilder
    {
        private Mock<ISaveCandidateStrategy> _saveCandidateStrategy = new Mock<ISaveCandidateStrategy>();
        private Mock<IMessageBus> _messageBus = new Mock<IMessageBus>();

        public QueuedLegacySaveCandidateStrategyBuilder()
        {
            _saveCandidateStrategy.Setup(s => s.SaveCandidate(It.IsAny<Candidate>())).Returns<Candidate>(c => c);
        }

        public ISaveCandidateStrategy Build()
        {
            var strategy = new QueuedLegacySaveCandidateStrategy(_saveCandidateStrategy.Object, _messageBus.Object);
            return strategy;
        }

        public QueuedLegacySaveCandidateStrategyBuilder With(Mock<ISaveCandidateStrategy> saveCandidateStrategy)
        {
            _saveCandidateStrategy = saveCandidateStrategy;
            return this;
        }

        public QueuedLegacySaveCandidateStrategyBuilder With(Mock<IMessageBus> messageBus)
        {
            _messageBus = messageBus;
            return this;
        }
    }
}