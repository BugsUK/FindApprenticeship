namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.Candidates;
    using Application.Interfaces.Candidates;
    using Builders;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class TrySaveSettingsTests
    {
        [TestCase("0123456789", CommunicationChannels.Email, false)]
        [TestCase("0987654321", CommunicationChannels.Sms, false)]
        [TestCase("0123456789", CommunicationChannels.Email, true)]
        [TestCase("0987654321", CommunicationChannels.Sms, true)]
        public void ShouldMapCommunicationPreferences(string phoneNumber, CommunicationChannels communicationChannel, bool verifiedMobile)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).PhoneNumber(phoneNumber).VerifiedMobile(verifiedMobile).Build);

            var viewModel = new SettingsViewModelBuilder()
                .PhoneNumber(phoneNumber)
                .EnableApplicationStatusChangeAlertsViaEmail(communicationChannel == CommunicationChannels.Email)
                .EnableApplicationStatusChangeAlertsViaText(communicationChannel == CommunicationChannels.Sms)
                .EnableExpiringApplicationAlertsViaEmail(communicationChannel == CommunicationChannels.Email)
                .EnableExpiringApplicationAlertsViaText(communicationChannel == CommunicationChannels.Sms)
                .EnableMarketingViaEmail(communicationChannel == CommunicationChannels.Email)
                .EnableMarketingViaText(communicationChannel == CommunicationChannels.Sms)
                .EnableSavedSearchAlertsViaEmail(communicationChannel == CommunicationChannels.Email)
                .EnableSavedSearchAlertsViaText(communicationChannel == CommunicationChannels.Sms)
                .Build();

            var provider = new AccountProviderBuilder().With(candidateService).Build();

            Candidate candidate;

            // Act.
            var result = provider.TrySaveSettings(candidateId, viewModel, out candidate);

            // Assert.
            result.Should().BeTrue();

            candidate.RegistrationDetails.Should().NotBeNull();
            candidate.RegistrationDetails.PhoneNumber.Should().Be(phoneNumber);

            candidate.CommunicationPreferences.Should().NotBeNull();

            {
                var preferences = candidate.CommunicationPreferences.ApplicationStatusChangePreferences;

                preferences.Should().NotBeNull();
                preferences.EnableEmail.Should().Be(communicationChannel == CommunicationChannels.Email);
                preferences.EnableText.Should().Be(communicationChannel == CommunicationChannels.Sms);
            }

            {
                var preferences = candidate.CommunicationPreferences.ExpiringApplicationPreferences;

                preferences.Should().NotBeNull();
                preferences.EnableEmail.Should().Be(communicationChannel == CommunicationChannels.Email);
                preferences.EnableText.Should().Be(communicationChannel == CommunicationChannels.Sms);
            }

            {
                var preferences = candidate.CommunicationPreferences.MarketingPreferences;

                preferences.Should().NotBeNull();
                preferences.EnableEmail.Should().Be(communicationChannel == CommunicationChannels.Email);
                preferences.EnableText.Should().Be(communicationChannel == CommunicationChannels.Sms);
            }

            {
                var preferences = candidate.CommunicationPreferences.SavedSearchPreferences;

                preferences.Should().NotBeNull();
                preferences.EnableEmail.Should().Be(communicationChannel == CommunicationChannels.Email);
                preferences.EnableText.Should().Be(communicationChannel == CommunicationChannels.Sms);
            }

            candidate.CommunicationPreferences.VerifiedMobile.Should().Be(verifiedMobile);
        }

        [TestCase("0123456789", false, false, false)]
        [TestCase("0123456789", true, false, false)]
        [TestCase("0123456789", false, true, true)]
        [TestCase("0123456789", true, true, false)]
        [TestCase("9876543210", false, false, false)]
        [TestCase("9876543210", true, false, false)]
        [TestCase("9876543210", false, true, true)]
        [TestCase("9876543210", true, true, true)]
        public void ShouldRequireMobileVerification(string newPhoneNumber, bool verifiedMobile, bool enableCommunicationViaText, bool mobileVerificationRequired)
        {
            // Arrange.
            const string mobileVerificationCode = "1234";
            
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();
            
            const string currentPhoneNumber = "0123456789";
            
            candidateService.Setup(cs => cs.GetCandidate(candidateId)).Returns(new CandidateBuilder(candidateId).PhoneNumber(currentPhoneNumber).VerifiedMobile(verifiedMobile).Build);
            candidateService.Setup(cs => cs.SaveCandidate(It.IsAny<Candidate>())).Returns<Candidate>(c =>
            {
                if (c.MobileVerificationRequired())
                    c.CommunicationPreferences.MobileVerificationCode = mobileVerificationCode;
                return c;
            });

            var viewModel = new SettingsViewModelBuilder().PhoneNumber(newPhoneNumber).EnableMarketingViaText(enableCommunicationViaText).Build();
            var provider = new AccountProviderBuilder().With(candidateService).Build();

            Candidate candidate;

            // Act.
            var result = provider.TrySaveSettings(candidateId, viewModel, out candidate);

            // Assert.
            result.Should().BeTrue();

            candidate.RegistrationDetails.Should().NotBeNull();
            candidate.RegistrationDetails.PhoneNumber.Should().Be(newPhoneNumber);

            candidate.CommunicationPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.MarketingPreferences.Should().NotBeNull();
            candidate.CommunicationPreferences.MarketingPreferences.EnableText.Should().Be(enableCommunicationViaText);

            if (newPhoneNumber != currentPhoneNumber)
            {
                candidate.CommunicationPreferences.VerifiedMobile.Should().BeFalse();
            }
            else
            {
                candidate.CommunicationPreferences.VerifiedMobile.Should().Be(verifiedMobile);
            }

            if (mobileVerificationRequired)
            {
                candidate.MobileVerificationRequired().Should().BeTrue();
                candidate.CommunicationPreferences.MobileVerificationCode.Should().Be(mobileVerificationCode);
            }
            else
            {
                candidate.MobileVerificationRequired().Should().BeFalse();
                candidate.CommunicationPreferences.MobileVerificationCode.Should().BeNullOrEmpty();
            }
        }

        [TestCase(0, null)]
        [TestCase(37, 37)]
        public void ShouldMapEthnicity(int ethnicity, int? expectedEthnicity)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService
                .Setup(cs => cs.GetCandidate(candidateId))
                .Returns(new CandidateBuilder(candidateId).Build);

            var viewModel = new SettingsViewModelBuilder()
                .PhoneNumber("0123456789")
                .Ethnicity(ethnicity)
                .Build();

            var provider = new AccountProviderBuilder().With(candidateService).Build();

            Candidate candidate;

            // Act.
            var result = provider.TrySaveSettings(candidateId, viewModel, out candidate);

            // Assert.
            result.Should().BeTrue();

            candidate.MonitoringInformation.Should().NotBeNull();
            candidate.MonitoringInformation.Ethnicity.Should().Be(expectedEthnicity);
        }

        [TestCase(null, null)]
        [TestCase(1, Gender.Male)]
        [TestCase(2, Gender.Female)]
        [TestCase(3, Gender.Other)]
        [TestCase(4, Gender.PreferNotToSay)]
        public void ShouldMapGender(int? gender, Gender? expectedGender)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService
                .Setup(cs => cs.GetCandidate(candidateId))
                .Returns(new CandidateBuilder(candidateId).Build);

            var viewModel = new SettingsViewModelBuilder()
                .PhoneNumber("0123456789")
                .Gender(gender)
                .Build();

            var provider = new AccountProviderBuilder().With(candidateService).Build();

            Candidate candidate;

            // Act.
            var result = provider.TrySaveSettings(candidateId, viewModel, out candidate);

            // Assert.
            result.Should().BeTrue();

            candidate.MonitoringInformation.Should().NotBeNull();
            candidate.MonitoringInformation.Gender.Should().Be(expectedGender);
        }

        [TestCase(null, null)]
        [TestCase(1, DisabilityStatus.Yes)]
        [TestCase(2, DisabilityStatus.No)]
        [TestCase(3, DisabilityStatus.PreferNotToSay)]
        public void ShouldMapDisabilityStatus(int? disabilityStatus, DisabilityStatus? expectedDisabilityStatus)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService
                .Setup(cs => cs.GetCandidate(candidateId))
                .Returns(new CandidateBuilder(candidateId).Build);

            var viewModel = new SettingsViewModelBuilder()
                .PhoneNumber("0123456789")
                .DisabilityStatus(disabilityStatus)
                .Build();

            var provider = new AccountProviderBuilder().With(candidateService).Build();

            Candidate candidate;

            // Act.
            var result = provider.TrySaveSettings(candidateId, viewModel, out candidate);

            // Assert.
            result.Should().BeTrue();

            candidate.MonitoringInformation.Should().NotBeNull();
            candidate.MonitoringInformation.DisabilityStatus.Should().Be(expectedDisabilityStatus);
        }

        [TestCase(false, false, null, null)]
        [TestCase(false, false, "A", "A")]
        [TestCase(false, true, "B", "B")]
        [TestCase(true, false, "C", null)]
        public void ShouldMapSupport(bool isJavascript, bool requiresSupport, string support, string expectedSuport)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var candidateService = new Mock<ICandidateService>();

            candidateService
                .Setup(cs => cs.GetCandidate(candidateId))
                .Returns(new CandidateBuilder(candidateId).Build);

            var viewModel = new SettingsViewModelBuilder()
                .IsJavascript(isJavascript)
                .Support(requiresSupport, support)
                .Build();

            var provider = new AccountProviderBuilder().With(candidateService).Build();

            Candidate candidate;

            // Act.
            var result = provider.TrySaveSettings(candidateId, viewModel, out candidate);

            // Assert.
            result.Should().BeTrue();
            candidate.MonitoringInformation.Should().NotBeNull();
            candidate.ApplicationTemplate.AboutYou.Support.Should().Be(expectedSuport);
        }
    }
}