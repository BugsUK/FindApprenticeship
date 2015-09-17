namespace SFA.Apprenticeships.Web.Recruit.Constants.Messages
{
    public class AuthorizeMessages
    {
        public const string EmptyUsername = "We have not been able to identify you successfully. Please contact helpdesk";
        public const string MissingProviderIdentifier = "We have not been able to identify your provider information. Please contact helpdesk";
        public const string MissingServicePermission = "You do not have permission to access this service. Please contact helpdesk";
        public const string NoProviderProfile = "As you're the first person to sign in from your organisation, please start by adding your provider name.";
        public const string FailedMinimumSitesCountCheck = "Please create at least one site";
        public const string NoUserProfile = "Please fill in your user profile";
        public const string EmailAddressNotVerified = "Please verify your email address";
    }
}