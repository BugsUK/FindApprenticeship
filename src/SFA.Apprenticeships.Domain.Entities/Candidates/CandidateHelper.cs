namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CandidateHelper
    {
        public static bool MobileVerificationRequired(this Candidate candidate)
        {
            var communicationPreferences = candidate.CommunicationPreferences;

            return !communicationPreferences.VerifiedMobile &&
                   (IsAnyTextCommunicationEnabled(communicationPreferences));
        }

        public static bool AllowsCommunication(this Candidate candidate)
        {
            var communicationPreferences = candidate.CommunicationPreferences;

            return IsAnyEmailCommunicationEnabled(communicationPreferences) ||
                   (IsAnyTextCommunicationEnabled(communicationPreferences) && communicationPreferences.VerifiedMobile);
        }

        public static bool ShouldSendSavedSearchAlerts(this Candidate candidate)
        {
            var communicationPreferences = candidate.CommunicationPreferences;
            var savedSearchPreferences = communicationPreferences.SavedSearchPreferences;

            return savedSearchPreferences.EnableEmail ||
                   (savedSearchPreferences.EnableText && communicationPreferences.VerifiedMobile);
        }

        #region Helpers

        private static bool IsAnyEmailCommunicationEnabled(CommunicationPreferences communicationPreferences)
        {
            return IndividualCommunicationPreferences(communicationPreferences).Any(each => each.EnableEmail);
        }

        private static bool IsAnyTextCommunicationEnabled(CommunicationPreferences communicationPreferences)
        {
            return IndividualCommunicationPreferences(communicationPreferences).Any(each => each.EnableText);
        }

        private static IEnumerable<CommunicationPreference> IndividualCommunicationPreferences(
            CommunicationPreferences communicationPreferences)
        {
            return new[]
            {
                communicationPreferences.ApplicationStatusChangePreferences,
                communicationPreferences.ExpiringApplicationPreferences,
                communicationPreferences.SavedSearchPreferences,
                communicationPreferences.MarketingPreferences
            };
        }

        #endregion
    }
}