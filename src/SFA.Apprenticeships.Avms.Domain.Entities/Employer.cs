namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employer")]
    public partial class Employer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employer()
        {
            VacancyOwnerRelationships = new HashSet<VacancyOwnerRelationship>();
            EmployerHistories = new HashSet<EmployerHistory>();
            EmployerSICCodes = new HashSet<EmployerSICCode>();
        }

        public int EmployerId { get; set; }

        public int EdsUrn { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        [StringLength(255)]
        public string TradingName { get; set; }

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

        [Required]
        [StringLength(8)]
        public string PostCode { get; set; }

        public int? LocalAuthorityId { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public int PrimaryContact { get; set; }

        public int? NumberofEmployeesAtSite { get; set; }

        public int? NumberOfEmployeesInGroup { get; set; }

        [StringLength(255)]
        public string OwnerOrgnistaion { get; set; }

        [StringLength(8)]
        public string CompanyRegistrationNumber { get; set; }

        public int? TotalVacanciesPosted { get; set; }

        [StringLength(50)]
        public string BeingSupportedBy { get; set; }

        public DateTime? LockedForSupportUntil { get; set; }

        public int? EmployerStatusTypeId { get; set; }

        public bool DisableAllowed { get; set; }

        public bool TrackingAllowed { get; set; }

        public virtual County County { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyOwnerRelationship> VacancyOwnerRelationships { get; set; }

        public virtual EmployerContact EmployerContact { get; set; }

        public virtual EmployerTrainingProviderStatu EmployerTrainingProviderStatu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployerHistory> EmployerHistories { get; set; }

        public virtual LocalAuthority LocalAuthority { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployerSICCode> EmployerSICCodes { get; set; }
    }
}
