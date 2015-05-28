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

        public static IEnumerable<CommunicationPreference> IndividualCommunicationPreferences(
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

        public static void EnableAllOptionalCommunications(this Candidate candidate)
        {
            foreach (var communicationPreference in IndividualCommunicationPreferences(candidate.CommunicationPreferences))
            {
                communicationPreference.EnableText = true;
                communicationPreference.EnableEmail = true;
            }
        }

        public static void DisableAllOptionalCommunications(this Candidate candidate)
        {
            foreach (var communicationPreference in IndividualCommunicationPreferences(candidate.CommunicationPreferences))
            {
                communicationPreference.EnableText = false;
                communicationPreference.EnableEmail = false;
            }
        }

        #endregion
    }
}