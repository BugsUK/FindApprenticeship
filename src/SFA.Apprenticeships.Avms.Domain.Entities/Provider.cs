namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Provider")]
    public partial class Provider
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Provider()
        {
            ProviderSiteRelationships = new HashSet<ProviderSiteRelationship>();
            SectorSuccessRates = new HashSet<SectorSuccessRate>();
            Vacancies = new HashSet<Vacancy>();
        }

        public int ProviderID { get; set; }

        public int UPIN { get; set; }

        public int UKPRN { get; set; }

        [StringLength(255)]
        public string FullName { get; set; }

        [StringLength(255)]
        public string TradingName { get; set; }

        public bool IsContracted { get; set; }

        public DateTime? ContractedFrom { get; set; }

        public DateTime? ContractedTo { get; set; }

        public int ProviderStatusTypeID { get; set; }

        public bool IsNASProvider { get; set; }

        public int? OriginalUPIN { get; set; }

        public virtual EmployerTrainingProviderStatu EmployerTrainingProviderStatu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderSiteRelationship> ProviderSiteRelationships { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SectorSuccessRate> SectorSuccessRates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
