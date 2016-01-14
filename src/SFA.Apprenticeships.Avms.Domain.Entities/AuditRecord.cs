namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuditRecord")]
    public partial class AuditRecord
    {
        public int AuditRecordId { get; set; }

        [Required]
        [StringLength(50)]
        public string Author { get; set; }

        public DateTime ChangeDate { get; set; }

        public int AttachedtoItem { get; set; }

        public int AttachedtoItemType { get; set; }

        public virtual AttachedtoItemType AttachedtoItemType1 { get; set; }
    }
}
