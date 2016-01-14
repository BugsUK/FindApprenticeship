namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdditionalQuestion")]
    public partial class AdditionalQuestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdditionalQuestion()
        {
            AdditionalAnswers = new HashSet<AdditionalAnswer>();
        }

        public int AdditionalQuestionId { get; set; }

        public int VacancyId { get; set; }

        public short QuestionId { get; set; }

        [Required]
        [StringLength(4000)]
        public string Question { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdditionalAnswer> AdditionalAnswers { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}
