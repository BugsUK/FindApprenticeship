namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies.Builders
{
    using Apprenticeships.Application.Candidate.Strategies;
    using Domain.Interfaces.Messaging;
    using Domain.Entities.Candidates;
    using Moq;

    public class QueuedLegacySaveCandidateStrategyBuilder
    {
        private Mock<ISaveCandidateStrategy> _saveCandidateStrategy = new Mock<ISaveCandidateStrategy>();
        private Mock<IServiceBus> _serviceBus = new Mock<IServiceBus>();

        public QueuedLegacySaveCandidateStrategyBuilder()
        {
            _saveCandidateStrategy.Setup(s => s.SaveCandidate(It.IsAny<Candidate>())).Returns<Candidate>(c => c);
        }

        public ISaveCandidateStrategy Build()
        {
            return new QueuedLegacySaveCandidateStrategy(_saveCandidateStrategy.Object, _serviceBus.Object);
        }

        public QueuedLegacySaveCandidateStrategyBuilder With(Mock<ISaveCandidateStrategy> saveCandidateStrategy)
        {
            _saveCandidateStrategy = saveCandidateStrategy;
            return this;
        }

        public QueuedLegacySaveCandidateStrategyBuilder With(Mock<IServiceBus> serviceBus)
        {
            _serviceBus = serviceBus;
            return this;
        }
    }
}