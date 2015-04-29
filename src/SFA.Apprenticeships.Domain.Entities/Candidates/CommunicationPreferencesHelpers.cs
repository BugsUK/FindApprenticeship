namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System.Collections.Generic;
    using System.Linq;

    // TODO: AG: US733: unit test.
    public static class CommunicationPreferencesHelpers
    {
        public static bool IsAnyEmailCommunicationEnabled(this CommunicationPreferences communicationPreferences)
        {
            return IndividualCommunicationPreferences(communicationPreferences).Any(each => each.EnableEmail);
        }

        public static bool IsAnyTextCommunicationEnabled(this CommunicationPreferences communicationPreferences)
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
    }
}
