namespace SFA.Apprenticeships.Application.UnitTests.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Communications;
    using Domain.Entities.Communication;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Interfaces.Communications;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class CommunicationRequestFactoryTests
    {
        [Test]
        public void ShouldCreateSavedSearchAlertCommunicationRequest()
        {            
            // Arrange.
            var candidate = new CandidateBuilder(new Guid())
                .Build();

            var savedSearchAlerts = GetSavedSearchAlerts(5, 3);

            // Act.
            var communicationRequest = CommunicationRequestFactory.GetSavedSearchAlertCommunicationRequest(
                candidate, savedSearchAlerts);

            // Assert.
            communicationRequest.Should().NotBeNull();
            communicationRequest.MessageType.Should().Be(MessageTypes.SavedSearchAlert);
            communicationRequest.EntityId.Should().Be(candidate.EntityId);
        }

        [Test]
        public void ShouldCreateSavedSearchAlertCommunicationRequestWithTokens()
        {
            // Arrange.
            var candidate = new CandidateBuilder(new Guid())
                .FirstName("Jane")
                .EmailAddress("jane.doe@example.com")
                .PhoneNumber("07999999999")
                .Build();

            var savedSearchAlerts = GetSavedSearchAlerts(5, 3);

            // Act.
            var communicationRequest = CommunicationRequestFactory.GetSavedSearchAlertCommunicationRequest(
                candidate, savedSearchAlerts);

            // Assert.

            // CandidateFirstName
            communicationRequest.ContainsToken(CommunicationTokens.CandidateFirstName).Should().BeTrue();

            communicationRequest.GetToken(CommunicationTokens.CandidateFirstName)
                .Should()
                .Be(candidate.RegistrationDetails.FirstName);

            // RecipientEmailAddress
            communicationRequest.ContainsToken(CommunicationTokens.RecipientEmailAddress).Should().BeTrue();

            communicationRequest.GetToken(CommunicationTokens.RecipientEmailAddress)
                .Should()
                .Be(candidate.RegistrationDetails.EmailAddress);

            // CandidateMobileNumber
            communicationRequest.ContainsToken(CommunicationTokens.CandidateMobileNumber).Should().BeTrue();

            communicationRequest.GetToken(CommunicationTokens.CandidateMobileNumber)
                .Should()
                .Be(candidate.RegistrationDetails.PhoneNumber);

            // SavedSearchAlerts
            communicationRequest.ContainsToken(CommunicationTokens.SavedSearchAlerts).Should().BeTrue();

            communicationRequest.GetToken(CommunicationTokens.SavedSearchAlerts)
                .Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void ShouldOrderSavedSearchAlertsByMostRecentlyCreated()
        {
            // Arrange.
            var candidate = new CandidateBuilder(new Guid())
                .Build();

            const int savedSearchAlertCount = 42;
            const int resultCount = 21;

            var savedSearchAlerts = GetSavedSearchAlerts(savedSearchAlertCount, resultCount);

            // Act.
            var communicationRequest = CommunicationRequestFactory.GetSavedSearchAlertCommunicationRequest(
                candidate, savedSearchAlerts);

            // Assert.
            communicationRequest.ContainsToken(CommunicationTokens.SavedSearchAlerts).Should().BeTrue();

            var json = communicationRequest.GetToken(CommunicationTokens.SavedSearchAlerts);
            var actualSavedSearchAlerts = JsonConvert.DeserializeObject<List<SavedSearchAlert>>(json);

            actualSavedSearchAlerts.Count.Should().Be(savedSearchAlertCount);
            actualSavedSearchAlerts.Should().BeInDescendingOrder(each => each.DateCreated);
        }

        [Test]
        public void ShouldOrderSavedSearchAlertResultsByMostRecentlyPostedVacancy()
        {
            // Arrange.
            var candidate = new CandidateBuilder(new Guid())
                .Build();

            const int savedSearchAlertCount = 42;
            const int resultCount = 21;

            var savedSearchAlerts = GetSavedSearchAlerts(savedSearchAlertCount, resultCount);

            // Act.
            var communicationRequest = CommunicationRequestFactory.GetSavedSearchAlertCommunicationRequest(
                candidate, savedSearchAlerts);

            // Assert.
            communicationRequest.ContainsToken(CommunicationTokens.SavedSearchAlerts).Should().BeTrue();

            var json = communicationRequest.GetToken(CommunicationTokens.SavedSearchAlerts);
            var actualSavedSearchAlerts = JsonConvert.DeserializeObject<List<SavedSearchAlert>>(json);

            foreach (var actualSavedSearchAlert in actualSavedSearchAlerts)
            {
                actualSavedSearchAlert.Results.Count.Should().Be(resultCount);
                actualSavedSearchAlert.Results.Should().BeInDescendingOrder(each => each.PostedDate);
            }
        }

        #region Helpers

        private static List<SavedSearchAlert> GetSavedSearchAlerts(int savedSearchAlertCount, int resultCount)
        {
            return new Fixture()
                .Build<SavedSearchAlert>()
                .With<IList<ApprenticeshipSearchResponse>>(each => each.Results,
                    new Fixture()
                        .Build<ApprenticeshipSearchResponse>()
                        .CreateMany(resultCount)
                        .ToList())
                .CreateMany(savedSearchAlertCount)
                .ToList();
        }

        #endregion
    }
}