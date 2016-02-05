﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // TODO: SQL: AG: choose UKPRN or ProviderId but not both.
    // TODO: SQL: AG: keep annotations (including Table)?

    [Table("Provider.ProviderUser")]
    public class ProviderUser
    {
        [Required]
        public int ProviderUserId { get; set; }

        [Required]
        public Guid ProviderUserGuid { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateUpdated { get; set; }

        [Required]
        public int ProviderUserStatusId { get; set; }

        [Required]
        public int Ukprn { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Fullname { get; set; }

        public int? PreferredSiteErn { get; set; }

        [Required]
        public string Email { get; set; }

        public string EmailVerificationCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EmailVerifiedDateTime { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
