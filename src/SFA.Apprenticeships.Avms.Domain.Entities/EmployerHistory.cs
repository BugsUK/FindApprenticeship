namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployerHistory")]
    public partial class EmployerHistory
    {
        public int EmployerHistoryId { get; set; }

        public int EmployerId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime Date { get; set; }

        public int Event { get; set; }

        [StringLength(4000)]
        public string Comment { get; set; }

        public virtual Employer Employer { get; set; }

        public virtual EmployerHistoryEventType EmployerHistoryEventType { get; set; }
    }
}
