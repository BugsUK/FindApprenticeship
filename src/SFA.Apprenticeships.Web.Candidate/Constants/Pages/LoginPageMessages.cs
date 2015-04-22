namespace SFA.Apprenticeships.Web.Candidate.Constants.Pages
{
    public class LoginPageMessages
    {
        public const string InvalidUsernameOrPasswordErrorText = "Please enter a valid email address or password";
        public const string LoginFailedErrorText = "There's been a problem signing in. Please try again.";
        public const string MobileVerificationRequiredText = "Your mobile number has not yet been verified. You won’t receive updates by text message until you <a href=\"{0}\">verify your number</a>.";
        public const string PendingUsernameVerificationRequiredText = "Your new email address has not yet been verified. You’ll be unable to sign in using your new email address until you <a href=\"{0}\">verify your email address</a>.";
        public const string ForgottenEmailSent = "We've sent your email address to your verified mobile number <b>{0}</b>.<br/><br/>If you don't receive a text message you may have entered an unverified phone number or a phone number we don't recognise.";
    }
}
