namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies
{
    using Apprenticeships.Application.Candidate;
    using Apprenticeships.Application.Candidate.Strategies;
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
            // Arrange.
            var saveCandidateStrategy = new Mock<ISaveCandidateStrategy>();

            saveCandidateStrategy.Setup(mock => mock.SaveCandidate(It.IsAny<Candidate>())).Returns<Candidate>(c => c);

            var strategy = new QueuedLegacySaveCandidateStrategyBuilder().With(saveCandidateStrategy).Build();
            var candidate = new Fixture().Build<Candidate>().Create();

            // Act.
            strategy.SaveCandidate(candidate);

            // Assert.
            saveCandidateStrategy.Verify(mock => mock.SaveCandidate(It.IsAny<Candidate>()), Times.Once);
        }

        [Test]
        public void ShouldQueueMessage()
        {
            // Arrange.
            var serviceBus = new Mock<IServiceBus>();

            SaveCandidateRequest request = null;

            serviceBus.Setup(b => b.PublishMessage(
                It.IsAny<SaveCandidateRequest>())).Callback<SaveCandidateRequest>(r => { request = r; });

            var strategy = new QueuedLegacySaveCandidateStrategyBuilder().With(serviceBus).Build();
            var candidate = new Fixture().Build<Candidate>().Create();

            // Act.
            strategy.SaveCandidate(candidate);

            // Assert.
            serviceBus.Verify(mock => mock.PublishMessage(It.IsAny<SaveCandidateRequest>()));
            request.Should().NotBeNull();
            request.CandidateId.Should().Be(candidate.EntityId);
        }
    }
}