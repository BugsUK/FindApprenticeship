namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Unsubscribe
{
    using System;
    using Application.Interfaces.Communications;
    using Candidate.Mediators.Unsubscribe;
    using Candidate.Providers;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class UnsubscribeMediatorTests
    {
        private Mock<ICandidateServiceProvider> _mockCandidateServiceProvider;

        [SetUp]
        public void SetUp()
        {
            _mockCandidateServiceProvider = new Mock<ICandidateServiceProvider>();
        }

        [TestCase(true, SubscriptionTypes.DailyDigestViaEmail, UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedShowAccountSettings)]
        [TestCase(false, SubscriptionTypes.DailyDigestViaEmail, UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedNotSignedIn)]
        [TestCase(true, SubscriptionTypes.SavedSearchAlertsViaEmail, UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedShowSavedSearchesSettings)]
        [TestCase(false, SubscriptionTypes.SavedSearchAlertsViaEmail, UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedNotSignedIn)]
        public void ShouldUnsubscribe(bool isCandidateSignedIn, SubscriptionTypes subscriptionType, string expectedMediatorCode)
        {
            // Arrange.
            var mediator = new UnsubscribeMediator(_mockCandidateServiceProvider.Object);
            var candidateId = isCandidateSignedIn ? Guid.NewGuid() : default(Guid?);
            var subscriberId = Guid.NewGuid();

            _mockCandidateServiceProvider.Setup(mock => mock
                .Unsubscribe(subscriberId, subscriptionType))
                .Returns(true);

            // Act.
            var response = mediator.Unsubscribe(candidateId, subscriberId, subscriptionType);

            // Assert.
            response.Should().NotBeNull();
            response.Code.Should().Be(expectedMediatorCode);
        }

        [Test]
        public void ShouldFailToUnsubscribe()
        {
            var mediator = new UnsubscribeMediator(_mockCandidateServiceProvider.Object);
            var subscriberId = Guid.NewGuid();

            const SubscriptionTypes subscriptionType = SubscriptionTypes.Unknown;

            _mockCandidateServiceProvider.Setup(mock => mock
                .Unsubscribe(subscriberId, subscriptionType))
                .Returns(false);

            // Act.
            var response = mediator.Unsubscribe(null, subscriberId, subscriptionType);

            // Assert.
            response.Should().NotBeNull();
            response.Code.Should().Be(UnsubscribeMediatorCodes.Unsubscribe.Error);
        }
    }
}