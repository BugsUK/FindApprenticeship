namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class VertifyUpdatedEmailViewModelMessages
    {
        public static class VerifyUpdatedEmailCodeMessages
        {
            public const string LabelText = "Enter code";
            public const string RequiredErrorText = "Please enter mobile verification code";
            public const string LengthErrorText = "Verification code must be 4 digits";
        }

        public static class PasswordMessages
        {
            public const string LabelText = "Enter your account password";
            public const string RequiredErrorText = "Please enter password";
            public const string LengthErrorText = "Password must be at least 8 characters long, contain upper and lowercase letters and one number";
            public const string WhiteListRegularExpression = Whitelists.PasswordWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Password " + Whitelists.PasswordWhitelist.ErrorText;
        }
    }
}
