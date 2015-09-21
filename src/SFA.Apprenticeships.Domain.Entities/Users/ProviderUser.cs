namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public class ProviderUser : BaseEntity
    {
        public string Username { get; set; }

        public string Ukprn { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public string PendingEmail { get; set; }

        public string VerificationCode { get; set; }

        public string PreferredSiteErn { get; set; }

        public string PhoneNumber { get; set; }
    }
}
