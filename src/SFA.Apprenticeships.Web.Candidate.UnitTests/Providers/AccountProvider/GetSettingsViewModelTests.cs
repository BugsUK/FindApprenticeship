namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Users;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    [Parallelizable]
    public class GetSettingsViewModelTests
    {
        [Test]
        public void TestUserMappings()
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).Build();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);

            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(ac => ac.GetUser(It.IsAny<Guid>()))
                .Returns(new User { Username = "username", PendingUsername = "pendingun" });

            var provider = new AccountProviderBuilder().With(candidateService).With(userAccountService).Build();

            var viewModel = provider.GetSettingsViewModel(candidateId);
            viewModel.Username.Should().Be("username");
            viewModel.PendingUsername.Should().Be("pendingun");
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void TestCommunicationMappings(bool verifiedMobile, bool enableEmail, bool enableText)
        {
            var candidateId = Guid.NewGuid();
            const string phoneNumber = "0123456789";

            var candidate = new CandidateBuilder(candidateId)
                .PhoneNumber(phoneNumber)
                .VerifiedMobile(verifiedMobile)
                .Build();

            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);

            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var viewModel = provider.GetSettingsViewModel(candidateId);

            viewModel.Should().NotBeNull();
            viewModel.PhoneNumber.Should().Be(phoneNumber);
            viewModel.VerifiedMobile.Should().Be(verifiedMobile);
        }

        [TestCase(false, false, false, false, CommunicationChannels.Email)]
        [TestCase(false, false, false, true, CommunicationChannels.Email)]
        [TestCase(false, false, true, false, CommunicationChannels.Email)]
        [TestCase(false, true, false, false, CommunicationChannels.Email)]
        [TestCase(true, false, false, false, CommunicationChannels.Email)]
        [TestCase(false, false, false, false, CommunicationChannels.Sms)]
        [TestCase(false, false, false, true, CommunicationChannels.Sms)]
        [TestCase(false, false, true, false, CommunicationChannels.Sms)]
        [TestCase(false, true, false, false, CommunicationChannels.Sms)]
        [TestCase(true, false, false, false, CommunicationChannels.Sms)]
        public void TestMarketingMappings(
            bool enableApplicationStatusChangeAlerts,
            bool enableExpiringApplicationAlerts,
            bool sendMarketingCommunications,
            bool sendSavedSearchAlerts,
            CommunicationChannels channel)
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId)
                .EnableApplicationStatusChangeAlertsViaEmail(enableApplicationStatusChangeAlerts && channel == CommunicationChannels.Email)
                .EnableApplicationStatusChangeAlertsViaText(enableApplicationStatusChangeAlerts && channel == CommunicationChannels.Sms)
                .EnableExpiringApplicationAlertsViaEmail(enableExpiringApplicationAlerts && channel == CommunicationChannels.Email)
                .EnableExpiringApplicationAlertsViaText(enableExpiringApplicationAlerts && channel == CommunicationChannels.Sms)
                .EnableMarketingViaEmail(sendMarketingCommunications && channel == CommunicationChannels.Email)
                .EnableMarketingViaText(sendMarketingCommunications && channel == CommunicationChannels.Sms)
                .EnableSavedSearchAlertsViaEmail(sendSavedSearchAlerts && channel == CommunicationChannels.Email)
                .EnableSavedSearchAlertsViaText(sendSavedSearchAlerts && channel == CommunicationChannels.Sms)
                .Build();

            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);

            var provider = new AccountProviderBuilder().With(candidateService).Build();
            var viewModel = provider.GetSettingsViewModel(candidateId);

            viewModel.Should().NotBeNull();

            viewModel.EnableApplicationStatusChangeAlertsViaEmail.Should().Be(enableApplicationStatusChangeAlerts && channel == CommunicationChannels.Email);
            viewModel.EnableApplicationStatusChangeAlertsViaText.Should().Be(enableApplicationStatusChangeAlerts && channel == CommunicationChannels.Sms);

            viewModel.EnableExpiringApplicationAlertsViaEmail.Should().Be(enableExpiringApplicationAlerts && channel == CommunicationChannels.Email);
            viewModel.EnableExpiringApplicationAlertsViaText.Should().Be(enableExpiringApplicationAlerts && channel == CommunicationChannels.Sms);

            viewModel.EnableMarketingViaEmail.Should().Be(sendMarketingCommunications && channel == CommunicationChannels.Email);
            viewModel.EnableMarketingViaText.Should().Be(sendMarketingCommunications && channel == CommunicationChannels.Sms);

            viewModel.EnableSavedSearchAlertsViaEmail.Should().Be(sendSavedSearchAlerts && channel == CommunicationChannels.Email);
            viewModel.EnableSavedSearchAlertsViaText.Should().Be(sendSavedSearchAlerts && channel == CommunicationChannels.Sms);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TestSavedSearchMappings(bool alertsEnabled)
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId).Build();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);
            var savedSearches = new Fixture().Build<SavedSearch>()
                .With(s => s.AlertsEnabled, alertsEnabled)
                .With(s => s.Keywords, string.Empty)
                .With(s => s.ApprenticeshipLevel, "All")
                .With(s => s.Location, "CV1 2WT")
                .With(s => s.Latitude, 1.1)
                .With(s => s.Longitude, 2.1)
                .With(s => s.WithinDistance, 5)
                .With(s => s.SubCategoriesFullName, "Surveying, Construction Civil Engineering")
                .With(s => s.DateProcessed, new DateTime(2015, 01, 01))
                .CreateMany(1).ToList();
            candidateService.Setup(cs => cs.GetSavedSearches(candidateId)).Returns(savedSearches);
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var viewModel = provider.GetSettingsViewModel(candidateId);

            candidateService.Verify(cs => cs.GetSavedSearches(candidateId), Times.Once);

            viewModel.Should().NotBeNull();
            viewModel.SavedSearches.Count.Should().Be(1);
            var savedSearch = viewModel.SavedSearches[0];
            savedSearch.Id.Should().Be(savedSearches[0].EntityId);
            savedSearch.Name.Should().Be("Within 5 miles of CV1 2WT");
            savedSearch.SearchUrl.Should().NotBeNull();
            savedSearch.SearchUrl.Value.Should().NotBeNullOrEmpty();
            savedSearch.AlertsEnabled.Should().Be(alertsEnabled);
            savedSearch.ApprenticeshipLevel.Should().Be("All");
            savedSearch.SubCategoriesFullNames.Should().Be("Surveying, Construction Civil Engineering");
            savedSearch.DateProcessed.HasValue.Should().BeTrue();
            savedSearch.DateProcessed.Should().Be(new DateTime(2015, 01, 01));
        }

        [TestCase(null, null, null, null)]
        [TestCase(Gender.Other, DisabilityStatus.PreferNotToSay, 37, "Braille")]
        public void TestMonitoringInformationMappings(
            Gender? gender, DisabilityStatus disabilityStatus, int? ethnicity, string support)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId)
                .With(gender)
                .With(disabilityStatus)
                .With(ethnicity)
                .With(new ApplicationTemplate
                {
                    AboutYou = new AboutYou
                    {
                        Support = support
                    }
                })
                .Build();

            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);

            var provider = new AccountProviderBuilder().With(candidateService).Build();

            // Act.
            var viewModel = provider.GetSettingsViewModel(candidateId);

            // Assert.
            viewModel.Should().NotBeNull();

            viewModel.MonitoringInformation.Gender.Should().Be((int?)gender);
            viewModel.MonitoringInformation.DisabilityStatus.Should().Be((int?)disabilityStatus);
            viewModel.MonitoringInformation.Ethnicity.Should().Be(ethnicity);
            viewModel.MonitoringInformation.RequiresSupportForInterview.Should().Be(!string.IsNullOrWhiteSpace(support));
            viewModel.MonitoringInformation.AnythingWeCanDoToSupportYourInterview.Should().Be(support);
        }
    }
}