namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class FeedbackViewModelMessages
    {
        public static class FullNameMessages
        {
            public const string LabelText = "Full name";
            public const string TooLongErrorText = "Full name mustn’t exceed 71 characters";
            public const string WhiteListRegularExpression = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Full name " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string TooLongErrorText = "Email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }

        public class DetailsMessages
        {
            public const string LabelText = "Your feedback";
            public const string RequiredErrorText = "Please enter your feedback";
            public const string TooLongErrorText = "Your question mustn’t exceed 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Your feedback " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}
