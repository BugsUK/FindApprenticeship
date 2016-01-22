namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Vacancy.Entities;

    [Table("Reference.Framework")]
    public class Framework
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Framework()
        {
            Vacancies = new HashSet<Vacancy>();
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

        public virtual FrameworkStatus FrameworkStatus { get; set; }

        public virtual Occupation Occupation { get; set; }

        public virtual Occupation Occupation1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
