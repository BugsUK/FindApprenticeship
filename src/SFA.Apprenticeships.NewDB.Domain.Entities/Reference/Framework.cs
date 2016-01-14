namespace SFA.Apprenticeships.NewDB.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reference.Framework")]
    public partial class Framework
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Framework()
        {
            Vacancies = new HashSet<Vacancy.Vacancy>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FrameworkId { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeName { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string FullName { get; set; }

        public int FrameworkStatusId { get; set; }

        public int OccupationId { get; set; }

        public int? PreviousOccupationId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClosedDate { get; set; }

        public virtual FrameworkStatu FrameworkStatu { get; set; }

        public virtual Occupation Occupation { get; set; }

        public virtual Occupation Occupation1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy.Vacancy> Vacancies { get; set; }
    }
}
