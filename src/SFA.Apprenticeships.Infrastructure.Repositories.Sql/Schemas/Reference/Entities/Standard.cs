namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Reference.Standard")]
    public class Standard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Standard()
        {
            //Vacancies = new HashSet<Vacancy>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StandardId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public int LarsCode { get; set; }

        [Required]
        public int StandardSectorId { get; set; }

        [Required]
        public int EducationLevelId { get; set; }

        [Required]
        public int ApprenticeshipFrameworkStatusTypeId { get; set; }

        //public virtual Level Level { get; set; }

        //public virtual Sector Sector { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
