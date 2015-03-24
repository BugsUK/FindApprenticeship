namespace SFA.Apprenticeships.Application.UserAccount.Configuration
{
    public class UserAccountConfiguration
    {
        public const string ConfigurationName = "UserAccount";

        public string UserDirectorySecretKey { get; set; }

        public int ActivationCodeExpiryDays { get; set; }

        public int MaximumPasswordAttemptsAllowed { get; set; }

        public int UnlockCodeExpiryDays { get; set; }

        public int PasswordResetCodeExpiryDays { get; set; }

        public string HelpdeskEmailAddress { get; set; }
    }
}
