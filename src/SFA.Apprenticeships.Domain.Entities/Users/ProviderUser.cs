namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    // TODO: AG: SQL: remove dead code below.

    public class ProviderUser : ICreatableEntity, IUpdatableEntity
    {
        public int ProviderUserId { get; set; }

        public Guid ProviderUserGuid { get; set; }

        public int ProviderId { get; set; }

        public ProviderUserStatuses Status { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public string EmailVerificationCode { get; set; }

        public DateTime? EmailVerifiedDate { get; set; }

        public string PreferredSiteErn { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }
    }
}
