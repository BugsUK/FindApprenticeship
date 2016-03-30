namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Vacancy.Entities;

    [Table("Reference.EducationLevel")]
    public class EducationLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EducationLevel()
        {
            //Standards = new HashSet<Standard>();
            //Vacancies = new HashSet<Vacancy>();
        }

        [Key]
        public int EducationLevelId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string CodeName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Standard> Standards { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
