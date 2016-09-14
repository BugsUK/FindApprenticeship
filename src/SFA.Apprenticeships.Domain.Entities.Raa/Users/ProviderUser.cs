namespace SFA.Apprenticeships.Domain.Entities.Raa.Users
{
    using System;

    public class ProviderUser : ICreatableEntity, IUpdatableEntity
    {
        public int ProviderUserId { get; set; }

        public Guid ProviderUserGuid { get; set; }

        public int ProviderId { get; set; }

        public ProviderUserStatus Status { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public string EmailVerificationCode { get; set; }

        public DateTime? EmailVerifiedDate { get; set; }

        public int? PreferredProviderSiteId { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int ReleaseNoteVersion { get; set; }
    }
}
