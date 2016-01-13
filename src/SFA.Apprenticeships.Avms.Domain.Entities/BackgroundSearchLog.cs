namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BackgroundSearchLog")]
    public partial class BackgroundSearchLog
    {
        public int BackgroundSearchLogId { get; set; }

        public DateTime Date { get; set; }

        public int NumberOfVacancies { get; set; }

        public int NumberOfCandidatesProcessed { get; set; }

        public int NumberOfFailures { get; set; }
    }
}
