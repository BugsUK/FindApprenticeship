namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SchoolAttended")]
    public partial class SchoolAttended
    {
        public int SchoolAttendedId { get; set; }

        public int CandidateId { get; set; }

        public int? SchoolId { get; set; }

        [StringLength(120)]
        public string OtherSchoolName { get; set; }

        [StringLength(120)]
        public string OtherSchoolTown { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ApplicationId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual School School { get; set; }
    }
}
