namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    public class ProviderUserSearchViewModelMessages
    {
        public const string NoSearchCriteriaErrorText = "You must enter at least one search criteria";

        public class Username
        {
            public const string LabelText = "Username (optional)";
        }

        public class Name
        {
            public const string LabelText = "Name (optional)";
        }

        public class Email
        {
            public const string LabelText = "Email (optional)";
        }

        public class AllUnverifiedEmails
        {
            public const string LabelText = "Return all accounts with unverified emails";
        }
    }
}