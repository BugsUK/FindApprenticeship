namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApplicationHistory")]
    public partial class ApplicationHistory
    {
        public int ApplicationHistoryId { get; set; }

        public int ApplicationId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime ApplicationHistoryEventDate { get; set; }

        public int ApplicationHistoryEventTypeId { get; set; }

        public int? ApplicationHistoryEventSubTypeId { get; set; }

        [StringLength(4000)]
        public string Comment { get; set; }

        public virtual Application Application { get; set; }

        public virtual ApplicationHistoryEvent ApplicationHistoryEvent { get; set; }
    }
}
