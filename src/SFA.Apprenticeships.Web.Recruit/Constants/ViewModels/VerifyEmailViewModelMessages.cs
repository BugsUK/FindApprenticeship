namespace SFA.Apprenticeships.Web.Recruit.Constants.ViewModels
{
    using Common.Constants;

    public class VerifyEmailViewModelMessages
    {

        public const string VerificationCodeEmailResentMessage = "We've emailed a verification code to {0}";
        public const string VerificationCodeEmailIncorrectMessage = "Verification code entered is incorrect";

        public static class VerificationCode
        {
            public const string LabelText = "Vertification code";
            public const string RequiredErrorText = "Please enter an verification code";
            public const string LengthErrorText = "Vertification code must be 6 characters";
            public const string WhiteListRegularExpression = Whitelists.CodeWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Vertification code " + Whitelists.CodeWhitelist.ErrorText;
        }
    }
}