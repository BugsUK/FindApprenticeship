namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExternalApplication")]
    public partial class ExternalApplication
    {
        public int ExternalApplicationId { get; set; }

        public int CandidateId { get; set; }

        public int VacancyId { get; set; }

        public DateTime ClickthroughDate { get; set; }

        public Guid? ExternalTrackingId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}
