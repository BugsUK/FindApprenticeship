namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public class User : BaseEntity<Guid>
    {
        public User()
        {
            LastActivity = DateTime.UtcNow;
        }

        private string _username;

        public string Username
        {
            get { return !string.IsNullOrWhiteSpace(_username) ? _username.ToLower() : null; }
            set { _username = value; }
        }

        public UserStatuses Status { get; set; }

        public UserRoles Roles { get; set; }

        public string ActivationCode { get; set; }

        public DateTime? ActivateCodeExpiry { get; set; }

        public DateTime? ActivationDate { get; set; }

        public int LoginIncorrectAttempts { get; set; }

        public string PasswordResetCode { get; set; }

        public DateTime? PasswordResetCodeExpiry { get; set; }

        public int PasswordResetIncorrectAttempts { get; set; }

        public string AccountUnlockCode { get; set; }

        public DateTime? AccountUnlockCodeExpiry { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime LastActivity { get; set; }

        private string _pendingUsername;

        public string PendingUsername
        {
            get { return !string.IsNullOrWhiteSpace(_pendingUsername) ? _pendingUsername.ToLower() : null; }
            set { _pendingUsername = value; }
        }

        public string PendingUsernameCode { get; set; }
    }
}
