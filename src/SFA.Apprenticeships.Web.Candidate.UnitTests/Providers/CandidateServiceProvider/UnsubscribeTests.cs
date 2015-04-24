namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Communications;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UnsubscribeTests
    {
        [TestCase(SubscriptionTypes.DailyDigestViaEmail, false, true)]
        [TestCase(SubscriptionTypes.SavedSearchAlertsViaEmail, true, false)]
        public void ShouldUnsubscribeFromDailyDigestViaEmail(
            int subscriptionTypeId,
            bool expectedDailyDigestViaEmail, 
            bool expectedSavedSearchAlertsViaEmail)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var subscriberId = Guid.NewGuid();

            var candidate = BuildSubscribedCandidate(candidateId);

            var mockCandidateService = new Mock<ICandidateService>();

            mockCandidateService
                .Setup(mock => mock.GetCandidateBySubscriberId(subscriberId))
                .Returns(candidate);

            var mockCandidateServiceProvider = new CandidateServiceProviderBuilder()
                .With(mockCandidateService)
                .Build();

            SubscriptionTypes subscriptionType;

            // Act.
            var unsubscribed = mockCandidateServiceProvider.TryUnsubscribe(subscriberId, subscriptionTypeId, out subscriptionType);

            // Assert.
            unsubscribed.Should().BeTrue();
            subscriptionType.Should().Be((SubscriptionTypes)subscriptionTypeId);

            var communicationPreferences = candidate.CommunicationPreferences;

            communicationPreferences.SendApplicationStatusChanges.Should().Be(expectedDailyDigestViaEmail);
            communicationPreferences.SendApplicationStatusChangesViaEmail.Should().Be(expectedDailyDigestViaEmail);

            communicationPreferences.SendApprenticeshipApplicationsExpiring.Should().Be(expectedDailyDigestViaEmail);
            communicationPreferences.SendApprenticeshipApplicationsExpiringViaEmail.Should().Be(expectedDailyDigestViaEmail);

            communicationPreferences.SendSavedSearchAlertsViaEmail.Should().Be(expectedSavedSearchAlertsViaEmail);
        }

        [Test]
        public void ShouldFailToUnsubscribeWhenSubscriptionTypeIsUnknown()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var subscriberId = Guid.NewGuid();

            var candidate = BuildSubscribedCandidate(candidateId);

            var mockCandidateService = new Mock<ICandidateService>();

            mockCandidateService
                .Setup(mock => mock.GetCandidateBySubscriberId(subscriberId))
                .Returns(candidate);

            var mockCandidateServiceProvider = new CandidateServiceProviderBuilder()
                .With(mockCandidateService)
                .Build();

            const int subscriptionTypeId = (int)SubscriptionTypes.Unknown;
            SubscriptionTypes subscriptionType;

            // Act.
            var unsubscribed = mockCandidateServiceProvider.TryUnsubscribe(subscriberId, subscriptionTypeId, out subscriptionType);

            // Assert.
            unsubscribed.Should().BeFalse();
        }

        [Test]
        public void ShouldFailToUnsubscribeWhenSubscriberIsUnknown()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var subscriberId = Guid.NewGuid();

            var candidate = BuildSubscribedCandidate(candidateId);

            var mockCandidateService = new Mock<ICandidateService>();

            mockCandidateService
                .Setup(mock => mock.GetCandidateBySubscriberId(subscriberId))
                .Throws<NotImplementedException>();

            var mockCandidateServiceProvider = new CandidateServiceProviderBuilder()
                .With(mockCandidateService)
                .Build();

            const int subscriptionTypeId = (int)SubscriptionTypes.DailyDigestViaEmail;
            SubscriptionTypes subscriptionType;

            // Act.
            var unsubscribed = mockCandidateServiceProvider.TryUnsubscribe(subscriberId, subscriptionTypeId, out subscriptionType);

            // Assert.
            unsubscribed.Should().BeFalse();
            subscriptionType.Should().Be((SubscriptionTypes)subscriptionTypeId);

            var communicationPreferences = candidate.CommunicationPreferences;

            communicationPreferences.SendApplicationStatusChanges.Should().BeTrue();
            communicationPreferences.SendApplicationStatusChangesViaEmail.Should().BeTrue();

            communicationPreferences.SendApprenticeshipApplicationsExpiring.Should().BeTrue();
            communicationPreferences.SendApprenticeshipApplicationsExpiringViaEmail.Should().BeTrue();

            communicationPreferences.SendSavedSearchAlertsViaEmail.Should().BeTrue();
        }

        #region Helpers

        private static Candidate BuildSubscribedCandidate(Guid candidateId)
        {
            return new CandidateBuilder(candidateId)
                .SendApplicationStatusChanges(true)
                .SendApprenticeshipApplicationsExpiring(true)
                .SendSavedSearchAlertsViaEmail(true)
                .Build();
        }

        #endregion
    }
}
