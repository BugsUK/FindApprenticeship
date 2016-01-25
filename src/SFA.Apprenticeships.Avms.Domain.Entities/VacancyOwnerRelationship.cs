namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VacancyOwnerRelationship")]
    public partial class VacancyOwnerRelationship
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VacancyOwnerRelationship()
        {
            Vacancies = new HashSet<Vacancy>();
            VacancyOwnerRelationshipHistories = new HashSet<VacancyOwnerRelationshipHistory>();
            ProviderSiteRelationships = new HashSet<ProviderSiteRelationship>();
        }

        public int VacancyOwnerRelationshipId { get; set; }

        public int EmployerId { get; set; }

        public int ProviderSiteID { get; set; }

        public bool ContractHolderIsEmployer { get; set; }

        public bool ManagerIsEmployer { get; set; }

        public int StatusTypeId { get; set; }

        [StringLength(4000)]
        public string Notes { get; set; }

        public string EmployerDescription { get; set; }

        [StringLength(256)]
        public string EmployerWebsite { get; set; }

        public int? EmployerLogoAttachmentId { get; set; }

        public bool NationWideAllowed { get; set; }

        public virtual AttachedDocument AttachedDocument { get; set; }

        public virtual Employer Employer { get; set; }

        public virtual ProviderSite ProviderSite { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }

        public virtual VacancyProvisionRelationshipStatusType VacancyProvisionRelationshipStatusType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyOwnerRelationshipHistory> VacancyOwnerRelationshipHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteRelationship> ProviderSiteRelationships { get; set; }
    }
}
