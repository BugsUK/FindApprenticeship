namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.UnsubscribeStrategy
{
    using System;
    using Apprenticeships.Application.Candidate.Strategies;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UnsubscribeSendSavedSearchAlertsViaEmailTests
    {
        private Mock<ILogService> _mockLogger;
        private Mock<ICandidateReadRepository> _mockCandidateRepository;
        private Mock<ISaveCandidateStrategy> _mockSaveCandidateStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogService>();
            _mockCandidateRepository = new Mock<ICandidateReadRepository>();
            _mockSaveCandidateStrategy = new Mock<ISaveCandidateStrategy>();
        }

        [Test]
        public void ShouldUnsubscribe()
        {
            // Arrange.
            var strategy = new UnsubscribeStrategy(
                _mockLogger.Object,
                _mockCandidateRepository.Object,
                _mockSaveCandidateStrategy.Object);

            var candidateId = Guid.NewGuid();
            var subscriberId = Guid.NewGuid();
            var candidate = BuildSubscribedCandidate(candidateId);

            _mockCandidateRepository.Setup(mock => mock
                .GetBySubscriberId(subscriberId, true))
                .Returns(candidate);

            // Act.
            var unsubscribed = strategy.Unsubscribe(subscriberId, SubscriptionTypes.SavedSearchAlertsViaEmail);
            
            // Assert.
            unsubscribed.Should().BeTrue();

            candidate.CommunicationPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.SavedSearchPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.SavedSearchPreferences.EnableEmail.Should().BeFalse();

            _mockSaveCandidateStrategy.Verify(mock => mock
                .SaveCandidate(candidate), Times.Once);

            _mockLogger.Verify(mock => mock
                .Info(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        #region Helpers

        private static Candidate BuildSubscribedCandidate(Guid candidateId)
        {
            return new CandidateBuilder(candidateId)
                .EnableAllCommunications()
                .Build();
        }

        #endregion
    }
}