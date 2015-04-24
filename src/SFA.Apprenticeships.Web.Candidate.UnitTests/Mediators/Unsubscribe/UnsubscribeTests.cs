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
    public class UnsubscribeTests
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
        public void ShouldUnsubscribe(bool isCandidateSignedIn, int subscriptionTypeId, string expectedMediatorCode)
        {
            // Arrange.
            var mediator = new UnsubscribeMediator(_mockCandidateServiceProvider.Object);
            var candidateId = isCandidateSignedIn ? Guid.NewGuid() : default(Guid?);
            var subscriberId = Guid.NewGuid();

            var subscriptionType = (SubscriptionTypes)subscriptionTypeId;

            _mockCandidateServiceProvider.Setup(mock => mock
                .TryUnsubscribe(subscriberId, subscriptionTypeId, out subscriptionType))
                .Returns(true);

            // Act.
            var response = mediator.Unsubscribe(candidateId, subscriberId, subscriptionTypeId);

            // Assert.
            response.Should().NotBeNull();
            response.Code.Should().Be(expectedMediatorCode);
            subscriptionType.Should().Be((SubscriptionTypes)subscriptionTypeId);
        }

        [Test]
        public void ShouldFailToUnsubscribe()
        {
            var mediator = new UnsubscribeMediator(_mockCandidateServiceProvider.Object);
            var subscriberId = Guid.NewGuid();

            const int subscriptionTypeId = 0;
            SubscriptionTypes subscriptionType;

            _mockCandidateServiceProvider.Setup(mock => mock
                .TryUnsubscribe(subscriberId, subscriptionTypeId, out subscriptionType))
                .Returns(false);

            // Act.
            var response = mediator.Unsubscribe(null, subscriberId, subscriptionTypeId);

            // Assert.
            response.Should().NotBeNull();
            response.Code.Should().Be(UnsubscribeMediatorCodes.Unsubscribe.Error);
        }
    }
}