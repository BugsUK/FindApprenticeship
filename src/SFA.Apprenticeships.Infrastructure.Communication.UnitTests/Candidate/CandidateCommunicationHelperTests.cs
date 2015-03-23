namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Candidate
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using NUnit.Framework;
    using Processes.Communications;

    [TestFixture]
    public class CandidateCommunicationHelperTests
    {
        [TestCase(CommunicationChannels.Email, MessageTypes.SendActivationCode, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.SendActivationCode, false)]
        [TestCase(CommunicationChannels.Email, MessageTypes.SendPasswordResetCode, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.SendPasswordResetCode, false)]
        [TestCase(CommunicationChannels.Email, MessageTypes.SendAccountUnlockCode, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.SendAccountUnlockCode, false)]
        [TestCase(CommunicationChannels.Email, MessageTypes.PasswordChanged, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.PasswordChanged, false)]
        public void ShouldSendMandatoryEmailsViaEmailOnly(
            CommunicationChannels communicationChannel, MessageTypes messageType, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications(false)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Email, MessageTypes.SendMobileVerificationCode, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.SendMobileVerificationCode, true)]
        public void ShouldSendMandatoryMobileVerificationCodeViaSmsOnly(
            CommunicationChannels communicationChannel, MessageTypes messageType, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications(false)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Email, MessageTypes.DailyDigest, false, false, false)]
        [TestCase(CommunicationChannels.Email, MessageTypes.DailyDigest, false, true, true)]
        [TestCase(CommunicationChannels.Email, MessageTypes.DailyDigest, true, false, true)]
        [TestCase(CommunicationChannels.Email, MessageTypes.DailyDigest, true, true, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.DailyDigest, true, true, false)]
        public void ShouldHonourDailyDigestPreferencesViaEmail(
            CommunicationChannels communicationChannel,
            MessageTypes messageType,
            bool sendApplicationStatusChanges,
            bool sendApprenticeshipApplicationsExpiring,
            bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .SendApplicationStatusChanges(sendApplicationStatusChanges)
                .SendApprenticeshipApplicationsExpiring(sendApprenticeshipApplicationsExpiring)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationSuccessful, false, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationSuccessful, true, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationSuccessful, true, true)]
        public void ShouldHonourSendApplicationStatusChangesPreferenceViaSms(
            CommunicationChannels communicationChannel,
            MessageTypes messageType,
            bool sendApplicationStatusChanges,
            bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .SendApplicationStatusChanges(sendApplicationStatusChanges)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationExpiringDraft, false, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationExpiringDraft, true, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary, false, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary, true, true)]
        [TestCase(CommunicationChannels.Email, MessageTypes.ApprenticeshipApplicationExpiringDraft, true, false)]
        [TestCase(CommunicationChannels.Email, MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary, true, false)]
        public void ShouldHonourSendApprenticeshipApplicationsExpiringPreferenceViaSms(
            CommunicationChannels communicationChannel,
            MessageTypes messageType,
            bool sendApprenticeshipApplicationsExpiring,
            bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .SendApprenticeshipApplicationsExpiring(sendApprenticeshipApplicationsExpiring)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Email, true, true)]
        [TestCase(CommunicationChannels.Email, false, false)]
        [TestCase(CommunicationChannels.Sms, true, true)]
        [TestCase(CommunicationChannels.Sms, false, false)]
        public void ShouldHonourSendApplicationSubmittedCommunicationPreference(
            CommunicationChannels communicationChannel, bool preference, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .SendApplicationSubmitted(preference)
                .Build();

            // Act.
            var sendApprenticeshipApplicationSubmitted = candidate.ShouldSendMessageViaChannel(
                communicationChannel, MessageTypes.ApprenticeshipApplicationSubmitted);

            var sendTraineeshipApplicationSubmitted = candidate.ShouldSendMessageViaChannel(
                communicationChannel, MessageTypes.TraineeshipApplicationSubmitted);

            // Assert.
            sendApprenticeshipApplicationSubmitted.Should().Be(expectedResult);
            sendTraineeshipApplicationSubmitted.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Email, true, true)]
        [TestCase(CommunicationChannels.Email, false, false)]
        public void ShouldHonourSendSavedSearchAlertsViaEmailCommunicationPreference(
            CommunicationChannels communicationChannel, bool preference, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .SendSavedSearchAlertsViaEmail(preference)
                .SendSavedSearchAlertsViaText(false)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, MessageTypes.SavedSearchAlert);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Sms, true, true)]
        [TestCase(CommunicationChannels.Sms, false, false)]
        public void ShouldHonourSendSavedSearchAlertsViaTextCommunicationPreference(
            CommunicationChannels communicationChannel, bool preference, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .SendSavedSearchAlertsViaEmail(false)
                .SendSavedSearchAlertsViaText(preference)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, MessageTypes.SavedSearchAlert);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void ShouldHonourGeneralCommunicationPreferencesForOptionalSmsMessages(
            bool allowMobile, bool verifiedMobile, bool expectedResult)
        {
            var messageTypes = new[]
            {
                MessageTypes.ApprenticeshipApplicationSuccessful,
                MessageTypes.ApprenticeshipApplicationExpiringDraft,
                MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary,
                MessageTypes.ApprenticeshipApplicationSubmitted,
                MessageTypes.TraineeshipApplicationSubmitted,
                MessageTypes.SavedSearchAlert
            };

            foreach (var messageType in messageTypes)
            {
                // Arrange.
                var candidate = new CandidateBuilder(Guid.NewGuid())
                    .AllowAllCommunications()
                    .AllowEmail(false)
                    .AllowMobile(allowMobile)
                    .VerifiedMobile(verifiedMobile)
                    .Build();

                // Act.
                var result = candidate.ShouldSendMessageViaChannel(
                    CommunicationChannels.Sms, messageType);

                // Assert.
                result.Should().Be(expectedResult);
            }
        }

        [TestCase(MessageTypes.DailyDigest, false, false)]
        [TestCase(MessageTypes.DailyDigest, true, true)]
        [TestCase(MessageTypes.SavedSearchAlert, false, false)]
        [TestCase(MessageTypes.SavedSearchAlert, true, true)]
        public void ShouldHonourGeneralCommunicationPreferencesForOptionalEmailMessages(
            MessageTypes messageType, bool allowEmail, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .AllowAllCommunications()
                .AllowEmail(allowEmail)
                .AllowMobile(false)
                .VerifiedMobile(false)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                CommunicationChannels.Email, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }
    }
}
