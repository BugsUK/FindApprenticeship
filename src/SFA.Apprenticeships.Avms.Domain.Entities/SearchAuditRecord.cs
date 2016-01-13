namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SearchAuditRecord")]
    public partial class SearchAuditRecord
    {
        public int SearchAuditRecordId { get; set; }

        public int CandidateId { get; set; }

        public DateTime RunDate { get; set; }

        [StringLength(500)]
        public string SearchCriteria { get; set; }

        public DateTime? RunTime { get; set; }

        public int? RecordCount { get; set; }
    }
}
