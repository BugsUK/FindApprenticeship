namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // TODO: SQL: AG: keep annotations (including Table)?

    [Table("Provider.ProviderUser")]
    public class ProviderUser
    {
        [Key]
        public int ProviderUserId { get; set; }

        [Required]
        public Guid ProviderUserGuid { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDateTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDateTime { get; set; }

        [Required]
        public int ProviderUserStatusId { get; set; }

        public int? PreferredProviderSiteId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public string Email { get; set; }

        public string EmailVerificationCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EmailVerifiedDateTime { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public int ReleaseNoteVersion { get; set; }

        public string Ukprn { get; set; }

        public string ProviderName { get; set; }
    }
}