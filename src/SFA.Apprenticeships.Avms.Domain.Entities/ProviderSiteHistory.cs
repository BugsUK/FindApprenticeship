namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProviderSiteHistory")]
    public partial class ProviderSiteHistory
    {
        [Key]
        public int TrainingProviderHistoryId { get; set; }

        public int TrainingProviderId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime HistoryDate { get; set; }

        public int EventTypeId { get; set; }

        [StringLength(4000)]
        public string Comment { get; set; }

        public virtual ProviderSite ProviderSite { get; set; }

        public virtual ProviderSiteHistoryEventType ProviderSiteHistoryEventType { get; set; }
    }
}
