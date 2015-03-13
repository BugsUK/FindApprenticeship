namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetSettingsViewModelTests
    {
        [TestCase(false, false, false)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        [TestCase(true, true, true)]
        public void TestCommunicationMappings(bool verifiedMobile, bool allowEmailComms, bool allowSmsComms)
        {
            var candidateId = Guid.NewGuid();
            const string phoneNumber = "0123456789";
            var candidate = new CandidateBuilder(candidateId)
                .PhoneNumber(phoneNumber)
                .AllowEmail(allowEmailComms)
                .AllowMobile(allowSmsComms)
                .VerifiedMobile(verifiedMobile)
                .Build();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var viewModel = provider.GetSettingsViewModel(candidateId);

            viewModel.Should().NotBeNull();
            viewModel.PhoneNumber.Should().Be(phoneNumber);
            viewModel.VerifiedMobile.Should().Be(verifiedMobile);
            viewModel.AllowEmailComms.Should().Be(allowEmailComms);
            viewModel.AllowSmsComms.Should().Be(allowSmsComms);
        }

        [TestCase(false, false, false, false, false)]
        [TestCase(true, true, true, true, true)]
        [TestCase(false, true, false, true, false)]
        [TestCase(true, false, true, false, true)]
        public void TestMarketingMappings(bool sendApplicationSubmitted, bool sendApplicationStatusChanges, bool sendApprenticeshipApplicationsExpiring, bool sendSavedSearchAlerts, bool sendMarketingCommunications)
        {
            var candidateId = Guid.NewGuid();
            var candidate = new CandidateBuilder(candidateId)
                .SendApplicationSubmitted(sendApplicationSubmitted)
                .SendApplicationStatusChanges(sendApplicationStatusChanges)
                .SendApprenticeshipApplicationsExpiring(sendApprenticeshipApplicationsExpiring)
                .SendSavedSearchAlerts(sendSavedSearchAlerts)
                .SendMarketingComms(sendMarketingCommunications)
                .Build();
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidate);
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var viewModel = provider.GetSettingsViewModel(candidateId);

            viewModel.Should().NotBeNull();
            viewModel.SendApplicationSubmitted.Should().Be(sendApplicationSubmitted);
            viewModel.SendApplicationStatusChanges.Should().Be(sendApplicationStatusChanges);
            viewModel.SendApprenticeshipApplicationsExpiring.Should().Be(sendApprenticeshipApplicationsExpiring);
            viewModel.SendSavedSearchAlerts.Should().Be(sendSavedSearchAlerts);
            viewModel.SendMarketingCommunications.Should().Be(sendMarketingCommunications);
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
                .With(s => s.WithinDistance, 5)
                .With(s => s.SubCategoriesFullName, "Surveying, Construction Civil Engineering")
                .CreateMany(1).ToList();
            candidateService.Setup(cs => cs.RetrieveSavedSearches(candidateId)).Returns(savedSearches);
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            var viewModel = provider.GetSettingsViewModel(candidateId);

            candidateService.Verify(cs => cs.RetrieveSavedSearches(candidateId), Times.Once);

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
        }
    }
}