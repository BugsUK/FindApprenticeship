namespace SFA.Apprenticeships.Infrastructure.Processes.Communications
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.Candidates;

    public static class CandidateCommunicationHelper
    {
        public static bool ShouldSendMessageViaChannel(
            this Candidate candidate, CommunicationChannels communicationChannel, MessageTypes messageType)
        {
            var communicationPreferences = candidate.CommunicationPreferences;

            var candidateAllowsCommunicationsViaChannel =
                (communicationChannel == CommunicationChannels.Email && communicationPreferences.AllowEmail) ||
                (communicationChannel == CommunicationChannels.Sms && communicationPreferences.AllowMobile && communicationPreferences.VerifiedMobile);

            switch (messageType)
            {
                case MessageTypes.SendActivationCode:
                case MessageTypes.SendPasswordResetCode:
                case MessageTypes.SendAccountUnlockCode:
                case MessageTypes.PasswordChanged:
                case MessageTypes.SendPendingUsernameCode:
                    // Currently sent via email only, candidate cannot opt out of these mandatory communications.
                    return communicationChannel == CommunicationChannels.Email;

                case MessageTypes.SendMobileVerificationCode:
                    return communicationChannel == CommunicationChannels.Sms;

                case MessageTypes.DailyDigest:
                    // Daily digests are sent via email only, candidate can opt out of application status alerts
                    // and expiring draft notifications individually.
                    return candidateAllowsCommunicationsViaChannel &&
                           communicationChannel == CommunicationChannels.Email &&
                           (communicationPreferences.SendApplicationStatusChanges ||
                            communicationPreferences.SendApprenticeshipApplicationsExpiring);

                case MessageTypes.ApprenticeshipApplicationSuccessful:
                case MessageTypes.ApprenticeshipApplicationUnsuccessful:
                case MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary:
                    // These application status alerts are sent via SMS only, candidate can opt out.
                    return communicationChannel == CommunicationChannels.Sms &&
                           candidateAllowsCommunicationsViaChannel &&
                           communicationPreferences.SendApplicationStatusChanges;

                case MessageTypes.ApprenticeshipApplicationExpiringDraft:
                case MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary:
                    // These expiring draft notifications are sent via SMS only, candidate can opt out.
                    return communicationChannel == CommunicationChannels.Sms &&
                           candidateAllowsCommunicationsViaChannel &&
                           communicationPreferences.SendApprenticeshipApplicationsExpiring;

                case MessageTypes.ApprenticeshipApplicationSubmitted:
                case MessageTypes.TraineeshipApplicationSubmitted:
                    // Application submitted notifications are sent via email only and candidate cannot opt out.
                    return communicationChannel == CommunicationChannels.Email;

                case MessageTypes.SavedSearchAlert:
                    // Saved search alerts may be sent via email and SMS, candidate can opt out of each channel separately.
                    return candidateAllowsCommunicationsViaChannel &&
                           ((communicationChannel == CommunicationChannels.Email &&
                             communicationPreferences.SendSavedSearchAlertsViaEmail) ||
                            (communicationChannel == CommunicationChannels.Sms &&
                             communicationPreferences.SendSavedSearchAlertsViaText));
            }

            // Do not allow any communications to be sent implicitly.
            return false;
        }
    }
}
