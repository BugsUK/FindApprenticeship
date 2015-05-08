namespace SFA.Apprenticeships.Infrastructure.Processes.Communications
{
    using Application.Interfaces.Communications;
    using Domain.Entities.Candidates;

    public static class CandidateCommunicationHelper
    {
        public static bool ShouldSendMessageViaChannel(
            this Candidate candidate, CommunicationChannels communicationChannel, MessageTypes messageType)
        {
            var communicationPreferences = candidate.CommunicationPreferences;

            switch (messageType)
            {
                case MessageTypes.SendActivationCode:
                case MessageTypes.SendPasswordResetCode:
                case MessageTypes.SendAccountUnlockCode:
                case MessageTypes.PasswordChanged:
                case MessageTypes.SendPendingUsernameCode:
                case MessageTypes.SendActivationCodeReminder:
                    // Currently sent via email only, candidate cannot opt out of these mandatory communications.
                    return communicationChannel == CommunicationChannels.Email;

                case MessageTypes.SendMobileVerificationCode:
                case MessageTypes.SendEmailReminder:
                    return communicationChannel == CommunicationChannels.Sms;

                case MessageTypes.DailyDigest:
                    // Daily digests are sent via email only, candidate can opt out of application status alerts
                    // and expiring draft notifications individually.
                    return communicationChannel == CommunicationChannels.Email &&
                           (communicationPreferences.ApplicationStatusChangePreferences.EnableEmail ||
                            communicationPreferences.ExpiringApplicationPreferences.EnableEmail);

                case MessageTypes.ApprenticeshipApplicationSuccessful:
                case MessageTypes.ApprenticeshipApplicationUnsuccessful:
                case MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary:
                    // These individual application status alerts are sent via SMS only, candidate can opt out.
                    // The email version is sent as an aggregate via the daily digest so that check is done above
                    return communicationChannel == CommunicationChannels.Sms &&
                           communicationPreferences.VerifiedMobile &&
                           communicationPreferences.ApplicationStatusChangePreferences.EnableText;

                case MessageTypes.ApprenticeshipApplicationExpiringDraft:
                case MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary:
                    // These expiring draft notifications are sent via SMS only, candidate can opt out.
                    return communicationChannel == CommunicationChannels.Sms &&
                           communicationPreferences.VerifiedMobile &&
                           communicationPreferences.ExpiringApplicationPreferences.EnableText;

                case MessageTypes.ApprenticeshipApplicationSubmitted:
                case MessageTypes.TraineeshipApplicationSubmitted:
                    // Application submitted notifications are sent via email only and candidate cannot opt out.
                    return communicationChannel == CommunicationChannels.Email;

                case MessageTypes.SavedSearchAlert:
                    // Saved search alerts may be sent via email and SMS, candidate can opt out of each channel separately.
                    return ((communicationChannel == CommunicationChannels.Email &&
                             communicationPreferences.SavedSearchPreferences.EnableEmail) ||
                            (communicationChannel == CommunicationChannels.Sms &&
                             communicationPreferences.VerifiedMobile &&
                             communicationPreferences.SavedSearchPreferences.EnableText));
            }

            // Do not allow any communications to be sent implicitly.
            return false;
        }
    }
}
