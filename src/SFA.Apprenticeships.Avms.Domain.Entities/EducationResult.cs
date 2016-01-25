namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EducationResult")]
    public partial class EducationResult
    {
        public int EducationResultId { get; set; }

        public int CandidateId { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        public int Level { get; set; }

        [StringLength(100)]
        public string LevelOther { get; set; }

        [StringLength(20)]
        public string Grade { get; set; }

        public DateTime? DateAchieved { get; set; }

        public int? ApplicationId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual EducationResultLevel EducationResultLevel { get; set; }
    }
}
