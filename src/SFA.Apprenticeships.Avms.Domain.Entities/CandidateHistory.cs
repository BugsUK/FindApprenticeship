namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CandidateHistory")]
    public partial class CandidateHistory
    {
        public int CandidateHistoryId { get; set; }

        public int CandidateId { get; set; }

        public int CandidateHistoryEventTypeId { get; set; }

        public int? CandidateHistorySubEventTypeId { get; set; }

        public DateTime EventDate { get; set; }

        [StringLength(4000)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual CandidateHistoryEvent CandidateHistoryEvent { get; set; }
    }
}
