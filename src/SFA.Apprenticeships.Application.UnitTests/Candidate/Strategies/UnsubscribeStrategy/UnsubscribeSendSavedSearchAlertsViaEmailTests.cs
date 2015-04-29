namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.UnsubscribeStrategy
{
    using System;
    using System.Collections.Generic;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.SavedSearches;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Interfaces.Logging;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UnsubscribeSendSavedSearchAlertsViaEmailTests
    {
        private Mock<ILogService> _mockLogger;
        private Mock<ICandidateReadRepository> _mockCandidateRepository;
        private Mock<ISaveCandidateStrategy> _mockSaveCandidateStrategy;
        private Mock<IRetrieveSavedSearchesStrategy> _mockRetrieveSavedSearchesStrategy;
        private Mock<IUpdateSavedSearchStrategy> _mockUpdateSavedSearchStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogService>();
            _mockCandidateRepository = new Mock<ICandidateReadRepository>();
            _mockSaveCandidateStrategy = new Mock<ISaveCandidateStrategy>();
            _mockRetrieveSavedSearchesStrategy = new Mock<IRetrieveSavedSearchesStrategy>();
            _mockUpdateSavedSearchStrategy = new Mock<IUpdateSavedSearchStrategy>();
        }

        [Test]
        public void ShouldUnsubscribe()
        {
            // Arrange.
            var strategy = new UnsubscribeStrategy(
                _mockLogger.Object,
                _mockCandidateRepository.Object,
                _mockSaveCandidateStrategy.Object,
                _mockRetrieveSavedSearchesStrategy.Object,
                _mockUpdateSavedSearchStrategy.Object);

            // Act.
            var candidateId = Guid.NewGuid();
            var subscriberId = Guid.NewGuid();
            var subscriberItemId = Guid.NewGuid();

            var candidate = BuildSubscribedCandidate(candidateId);

            _mockCandidateRepository.Setup(mock => mock
                .GetBySubscriberId(subscriberId, true))
                .Returns(candidate);

            var savedSearch = new SavedSearch
            {
                EntityId = subscriberItemId,
                AlertsEnabled = true
            };

            _mockRetrieveSavedSearchesStrategy.Setup(mock => mock
                .RetrieveSavedSearches(candidateId))
                .Returns(new List<SavedSearch>
                {
                    savedSearch
                });

            var unsubscribed = strategy.Unsubscribe(
                subscriberId, SubscriptionTypes.SavedSearchAlertsViaEmail, subscriberItemId.ToString());
            
            // Assert.
            unsubscribed.Should().BeTrue();

            savedSearch.AlertsEnabled.Should().Be(false);

            _mockUpdateSavedSearchStrategy.Verify(mock => mock
                .UpdateSavedSearch(savedSearch), Times.Once);

            _mockLogger.Verify(mock => mock
                .Info(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void ShouldNotUnsubscribeIfSavedSearchNotFound()
        {
            // Arrange.
            var strategy = new UnsubscribeStrategy(
                _mockLogger.Object,
                _mockCandidateRepository.Object,
                _mockSaveCandidateStrategy.Object,
                _mockRetrieveSavedSearchesStrategy.Object,
                _mockUpdateSavedSearchStrategy.Object);

            // Act.
            var candidateId = Guid.NewGuid();
            var subscriberId = Guid.NewGuid();
            var subscriberItemId = Guid.NewGuid();

            var candidate = BuildSubscribedCandidate(candidateId);

            _mockCandidateRepository.Setup(mock => mock
                .GetBySubscriberId(subscriberId, true))
                .Returns(candidate);

            _mockRetrieveSavedSearchesStrategy.Setup(mock => mock
                .RetrieveSavedSearches(candidateId))
                .Returns(new List<SavedSearch>());

            var unsubscribed = strategy.Unsubscribe(
                subscriberId, SubscriptionTypes.SavedSearchAlertsViaEmail, subscriberItemId.ToString());

            // Assert.
            unsubscribed.Should().BeFalse();

            _mockUpdateSavedSearchStrategy.Verify(mock => mock
                .UpdateSavedSearch(It.IsAny<SavedSearch>()), Times.Never);

            _mockLogger.Verify(mock => mock
                .Error(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
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