namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using NUnit.Framework.Constraints;

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
    }
}