namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vacancy.VacancyPartyRelationshipType")]
    public class VacancyPartyRelationshipType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VacancyPartyRelationshipType()
        {
            VacancyPartyRelationships = new HashSet<VacancyPartyRelationship>();
        }

        [Key]
        [StringLength(3)]
        public string VacancyPartyRelationshipTypeCode { get; set; }

        [Required]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyPartyRelationship> VacancyPartyRelationships { get; set; }
    }
}
