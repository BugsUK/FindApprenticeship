namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Address.Entities;

    [Table("Vacancy.VacancyParty")]
    public partial class VacancyParty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VacancyParty()
        {
            Vacancies = new HashSet<Vacancy>();
            Vacancies1 = new HashSet<Vacancy>();
            Vacancies2 = new HashSet<Vacancy>();
            Vacancies3 = new HashSet<Vacancy>();
            Vacancies4 = new HashSet<Vacancy>();
            Vacancies5 = new HashSet<Vacancy>();
            VacancyPartyRelationships = new HashSet<VacancyPartyRelationship>();
            VacancyPartyRelationships1 = new HashSet<VacancyPartyRelationship>();
        }

        public int VacancyPartyId { get; set; }

        [Required]
        [StringLength(3)]
        public string VacancyPartyTypeCode { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Description { get; set; }

        public int? PostalAddressId { get; set; }

        public string WebsiteUrl { get; set; }

        public int? EdsErn { get; set; }

        public int? UKPrn { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies4 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies5 { get; set; }

        public virtual VacancyPartyType VacancyPartyType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyPartyRelationship> VacancyPartyRelationships { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyPartyRelationship> VacancyPartyRelationships1 { get; set; }
    }
}
