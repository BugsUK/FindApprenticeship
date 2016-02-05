namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    // TODO: SQL: AG: should be BaseEntity<int>.
    public class ProviderUser : BaseEntity<Guid>
    {
        public ProviderUser()
        {
            Status = ProviderUserStatuses.Registered;
        }

        // TODO: SQL: AG: following field is temporary ahead of migrating to BaseEntity<int>.
        public int ProviderUserId { get; set; }

        public Guid ProviderUserGuid { get; set; }

        public string Username { get; set; }

        public int ProviderId { get; set; }

        public string Ukprn { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public string EmailVerificationCode { get; set; }

        public DateTime? EmailVerifiedDate { get; set; }

        public string PreferredSiteErn { get; set; }

        public string PhoneNumber { get; set; }

        public ProviderUserStatuses Status { get; set; }
    }
}
