namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    public static class CandidateHelper
    {
        public static bool MobileVerificationRequired(this Candidate candidate)
        {
            var communicationPreferences = candidate.CommunicationPreferences;
            return !communicationPreferences.VerifiedMobile && (communicationPreferences.AllowMobile || communicationPreferences.SendSavedSearchAlertsViaText);
        }

        public static bool AllowsCommunication(this Candidate candidate)
        {
            var communicationPreferences = candidate.CommunicationPreferences;

            return communicationPreferences.AllowEmail || (communicationPreferences.AllowMobile && communicationPreferences.VerifiedMobile);
        }

        public static bool ShouldSendSavedSearchAlerts(this Candidate candidate)
        {
            var communicationPreferences = candidate.CommunicationPreferences;
            return communicationPreferences.SendSavedSearchAlertsViaEmail || (communicationPreferences.SendSavedSearchAlertsViaText && communicationPreferences.AllowMobile && communicationPreferences.VerifiedMobile);
        }
    }
}