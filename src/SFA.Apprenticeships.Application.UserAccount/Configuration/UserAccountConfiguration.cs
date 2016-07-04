namespace SFA.Apprenticeships.Application.UserAccount.Configuration
{
    public class UserAccountConfiguration
    {
        public string UserDirectorySecretKey { get; set; }

        public int ActivationCodeExpiryDays { get; set; }

        public int MaximumPasswordAttemptsAllowed { get; set; }

        public int UnlockCodeExpiryDays { get; set; }

        public int PasswordResetCodeExpiryDays { get; set; }

        public string HelpdeskEmailAddress { get; set; }

        public string RecruitHelpdeskEmailAddress { get; set; }

        public string FeedbackEmailAddress { get; set; }

        public string NoReplyEmailAddress { get; set; }
    }
}