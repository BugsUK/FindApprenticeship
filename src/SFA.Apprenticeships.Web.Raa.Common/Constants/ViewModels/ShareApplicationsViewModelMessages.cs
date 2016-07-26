namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    using Web.Common.Constants;

    public class ShareApplicationsViewModelMessages
    {
        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string RequiredErrorText = "Enter email address";
            public const string TooLongErrorText = "Email address must not be more than 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }
    }
}
