using SFA.Apprenticeships.Web.Common.Constants;

namespace SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels
{
    public class VerifyEmailViewModelMessages
    {

        public const string VerificationCodeEmailResentMessage = "We've emailed a verification code to {0}";
        public const string VerificationCodeEmailResentFailedMessage = "An error occured when emailing your verification code";
        public const string VerificationCodeEmailIncorrectMessage = "Verification code entered is incorrect";

        public static class VerificationCode
        {
            public const string LabelText = "Verification code";
            public const string RequiredErrorText = "Please enter an verification code";
            public const string LengthErrorText = "Verification code must be 6 characters";
            public const string WhiteListRegularExpression = Whitelists.CodeWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Verification code " + Whitelists.CodeWhitelist.ErrorText;
        }
    }
}