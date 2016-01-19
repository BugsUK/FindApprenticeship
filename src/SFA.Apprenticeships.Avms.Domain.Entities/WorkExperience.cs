namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WorkExperience")]
    public partial class WorkExperience
    {
        public int WorkExperienceId { get; set; }

        public int CandidateId { get; set; }

        [Required]
        [StringLength(50)]
        public string Employer { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(200)]
        public string TypeOfWork { get; set; }

        public bool PartialCompletion { get; set; }

        public bool VoluntaryExperience { get; set; }

        public int? ApplicationId { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}
