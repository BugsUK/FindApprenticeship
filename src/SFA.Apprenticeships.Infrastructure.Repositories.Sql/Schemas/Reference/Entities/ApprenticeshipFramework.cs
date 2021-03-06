namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.ApprenticeshipFramework")]
    public class ApprenticeshipFramework
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApprenticeshipFramework()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ApprenticeshipFrameworkId { get; set; }

        [Required]
        public int ApprenticeshipOccupationId { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeName { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public int ApprenticeshipFrameworkStatusTypeId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClosedDate { get; set; }

        public int? PreviousApprenticeshipOccupationId { get; set; }
    }
}
