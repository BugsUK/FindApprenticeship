namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public class ProviderUser : BaseEntity<Guid>
    {
        public ProviderUser()
        {
            Status = ProviderUserStatuses.Registered;
        }

        public string Username { get; set; }

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
