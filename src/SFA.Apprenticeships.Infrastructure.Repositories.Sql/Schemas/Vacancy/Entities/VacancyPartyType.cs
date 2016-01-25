namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vacancy.VacancyPartyType")]
    public class VacancyPartyType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VacancyPartyType()
        {
            VacancyParties = new HashSet<VacancyParty>();
        }

        [Key]
        [StringLength(3)]
        public string VacancyPartyTypeCode { get; set; }

        [Required]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyParty> VacancyParties { get; set; }
    }
}
