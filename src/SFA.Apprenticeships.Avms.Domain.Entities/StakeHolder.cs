namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StakeHolder")]
    public partial class StakeHolder
    {
        public int StakeHolderID { get; set; }

        public int PersonId { get; set; }

        public int StakeHolderStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressLine1 { get; set; }

        [StringLength(50)]
        public string AddressLine2 { get; set; }

        [StringLength(50)]
        public string AddressLine3 { get; set; }

        [StringLength(50)]
        public string AddressLine4 { get; set; }

        [StringLength(50)]
        public string AddressLine5 { get; set; }

        [Required]
        [StringLength(50)]
        public string Town { get; set; }

        public int CountyId { get; set; }

        [StringLength(100)]
        public string UnconfirmedEmailAddress { get; set; }

        [Required]
        [StringLength(8)]
        public string Postcode { get; set; }

        public int OrganisationId { get; set; }

        [StringLength(50)]
        public string OrganisationOther { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public DateTime? LastAccessedDate { get; set; }

        public bool ForgottenUsernameRequested { get; set; }

        public bool ForgottenPasswordRequested { get; set; }

        public bool EmailAlertSent { get; set; }

        [StringLength(50)]
        public string BeingSupportedBy { get; set; }

        public DateTime? LockedForSupportUntil { get; set; }

        public int? LocalAuthorityId { get; set; }

        public virtual County County { get; set; }

        public virtual LocalAuthority LocalAuthority { get; set; }

        public virtual Organisation Organisation { get; set; }

        public virtual Person Person { get; set; }

        public virtual StakeHolderStatu StakeHolderStatu { get; set; }
    }
}
