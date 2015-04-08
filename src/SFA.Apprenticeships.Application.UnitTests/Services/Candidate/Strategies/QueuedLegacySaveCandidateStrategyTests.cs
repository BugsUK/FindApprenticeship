namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies
{
    using Application.Candidate;
    using Application.Candidate.Strategies;
    using Builders;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class QueuedLegacySaveCandidateStrategyTests
    {
        [Test]
        public void ShouldUpdateCandidateSystem()
        {
            var saveCandidateStrategy = new Mock<ISaveCandidateStrategy>();
            saveCandidateStrategy.Setup(s => s.SaveCandidate(It.IsAny<Candidate>())).Returns<Candidate>(c => c);
            var strategy = new QueuedLegacySaveCandidateStrategyBuilder().With(saveCandidateStrategy).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            strategy.SaveCandidate(candidate);

            saveCandidateStrategy.Verify(p => p.SaveCandidate(It.IsAny<Candidate>()), Times.Once);
        }

        [Test]
        public void ShouldQueueMessage()
        {
            var messageBus = new Mock<IMessageBus>();
            SaveCandidateRequest request = null;
            messageBus.Setup(b => b.PublishMessage(It.IsAny<SaveCandidateRequest>())).Callback<SaveCandidateRequest>(r => { request = r; });

            var strategy = new QueuedLegacySaveCandidateStrategyBuilder().With(messageBus).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            strategy.SaveCandidate(candidate);

            messageBus.Verify(b => b.PublishMessage(It.IsAny<SaveCandidateRequest>()));
            request.Should().NotBeNull();
            request.CandidateId.Should().Be(candidate.EntityId);
        }
    }
}