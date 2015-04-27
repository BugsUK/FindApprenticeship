namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.UnsubscribeStrategy
{
    using System;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.SavedSearches;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Interfaces.Logging;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UnsubscribeFailureTests
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
        public void ShouldFailToUnsubscribeIfCandidateNotFound()
        {
            // Arrange.
            var strategy = new UnsubscribeStrategy(
                _mockLogger.Object,
                _mockCandidateRepository.Object,
                _mockSaveCandidateStrategy.Object,
                _mockRetrieveSavedSearchesStrategy.Object,
                _mockUpdateSavedSearchStrategy.Object);

            // Act.
            _mockCandidateRepository.Setup(mock => mock
                .GetBySubscriberId(It.IsAny<Guid>(), true))
                .Throws<NotImplementedException>();

            var unsubscribed = strategy.Unsubscribe(new Guid(), SubscriptionTypes.SavedSearchAlertsViaEmail);

            // Assert.
            unsubscribed.Should().BeFalse();

            _mockLogger.Verify(mock => mock
                .Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<object[]>()), Times.Once);
        }
    }
}
