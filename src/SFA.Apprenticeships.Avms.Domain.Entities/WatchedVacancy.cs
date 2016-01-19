namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WatchedVacancy")]
    public partial class WatchedVacancy
    {
        public int WatchedVacancyId { get; set; }

        public int CandidateId { get; set; }

        public int VacancyId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}
