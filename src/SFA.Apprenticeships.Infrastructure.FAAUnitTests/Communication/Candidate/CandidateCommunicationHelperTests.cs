namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Candidate
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Infrastructure.Processes.Communications;
    using NUnit.Framework;

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
        [TestCase(CommunicationChannels.Email, MessageTypes.SendPendingUsernameCode, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.SendPendingUsernameCode, false)]
        public void ShouldSendMandatoryEmailsViaEmailOnly(
            CommunicationChannels communicationChannel, MessageTypes messageType, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications(false)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Email, MessageTypes.SendMobileVerificationCode, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.SendMobileVerificationCode, true)]
        [TestCase(CommunicationChannels.Email, MessageTypes.SendMobileVerificationCodeReminder, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.SendMobileVerificationCodeReminder, true)]
        public void ShouldSendMandatoryMobileVerificationCodeViaSmsOnly(
            CommunicationChannels communicationChannel, MessageTypes messageType, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications(false)
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
            bool sendApplicationStatusChangesViaEmail,
            bool sendApprenticeshipApplicationsExpiringViaEmail,
            bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .EnableApplicationStatusChangeAlertsViaEmail(sendApplicationStatusChangesViaEmail)
                .EnableApplicationStatusChangeAlertsViaText(true)
                .EnableExpiringApplicationAlertsViaEmail(sendApprenticeshipApplicationsExpiringViaEmail)
                .EnableExpiringApplicationAlertsViaText(true)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationSuccessful, false, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationSuccessful, true, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationUnsuccessful, false, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationUnsuccessful, true, true)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary, false, false)]
        [TestCase(CommunicationChannels.Sms, MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary, true, true)]
        public void ShouldHonourSendApplicationStatusChangesPreferenceViaSms(
            CommunicationChannels communicationChannel,
            MessageTypes messageType,
            bool sendApplicationStatusChangesViaText,
            bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .EnableApplicationStatusChangeAlertsViaText(sendApplicationStatusChangesViaText)
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
            bool sendApprenticeshipApplicationsExpiringViaText,
            bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
                .EnableExpiringApplicationAlertsViaText(sendApprenticeshipApplicationsExpiringViaText)
                .EnableExpiringApplicationAlertsViaEmail(true)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, messageType);

            // Assert.
            result.Should().Be(expectedResult);
        }

        [TestCase(CommunicationChannels.Email, true, true)]
        [TestCase(CommunicationChannels.Email, false, true)]
        [TestCase(CommunicationChannels.Sms, true, false)]
        [TestCase(CommunicationChannels.Sms, false, false)]
        public void ShouldHonourSendApplicationSubmittedCommunicationPreference(
            CommunicationChannels communicationChannel, bool preference, bool expectedResult)
        {
            // Arrange.
            var candidate = new CandidateBuilder(Guid.NewGuid())
                .EnableAllCommunications()
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
                .EnableAllCommunications()
                .EnableSavedSearchAlertsViaEmail(preference)
                .EnableSavedSearchAlertsViaText(false)
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
                .EnableAllCommunications()
                .EnableSavedSearchAlertsViaEmail(false)
                .EnableSavedSearchAlertsViaText(preference)
                .Build();

            // Act.
            var result = candidate.ShouldSendMessageViaChannel(
                communicationChannel, MessageTypes.SavedSearchAlert);

            // Assert.
            result.Should().Be(expectedResult);
        }
    }
}
