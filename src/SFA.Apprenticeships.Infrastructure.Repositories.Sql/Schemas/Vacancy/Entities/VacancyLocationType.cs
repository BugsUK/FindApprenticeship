namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vacancy.VacancyLocationType")]
    public class VacancyLocationType
    {
        public const string Employer = "E";
        public const string Specific = "S";
        public const string Multiple = "M";
        public const string Nationwide = "N";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VacancyLocationType()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        [Key]
        [StringLength(1)]
        public string VacancyLocationTypeCode { get; set; }

        [Required]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
