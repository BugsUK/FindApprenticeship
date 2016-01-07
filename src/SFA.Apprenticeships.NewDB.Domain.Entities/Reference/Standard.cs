namespace SFA.Apprenticeships.NewDB.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reference.Standard")]
    public partial class Standard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Standard()
        {
            Vacancies = new HashSet<Vacancy.Vacancy>();
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
        public virtual ICollection<Vacancy.Vacancy> Vacancies { get; set; }
    }
}
