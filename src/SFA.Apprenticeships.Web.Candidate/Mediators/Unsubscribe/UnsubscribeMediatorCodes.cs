namespace SFA.Apprenticeships.Web.Candidate.Mediators.Unsubscribe
{
    public static class UnsubscribeMediatorCodes
    {
        // TODO: US733: 
        // Daily digest:
        //      not signed in -> Sign In
        //      signed in -> Account Settings

        // Saved search alerts:
        //      not signed in -> Sign In
        //      signed in -> Saved searches

        // UnsubscribedNotSignedIn
        // UnsubscribedShowSavedSearchesSettings

        public static class Unsubscribe
        {
            public const string Error = "UnsubscribeMediatorCodes.Unsubscribe.Error";
            public const string UnsubscribedNotSignedIn = "UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedNotSignedIn";
            public const string UnsubscribedShowAccountSettings = "UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedShowAccountSettings";
            public const string UnsubscribedShowSavedSearchesSettings = "UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedShowSavedSearchesSettings";
        }
    }
}