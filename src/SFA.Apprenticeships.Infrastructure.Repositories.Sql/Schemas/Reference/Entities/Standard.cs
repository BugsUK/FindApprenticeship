namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Vacancy.Entities;

    [Table("Reference.Standard")]
    public class Standard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Standard()
        {
            Vacancies = new HashSet<Vacancy>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StandardId { get; set; }

        [Required]
        public string FullName { get; set; }

        public int SectorId { get; set; }

        [Required]
        [StringLength(1)]
        public string LevelCode { get; set; }

        public virtual Level Level { get; set; }

        public virtual Sector Sector { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
