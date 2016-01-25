namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VacancyOwnerRelationshipHistory")]
    public partial class VacancyOwnerRelationshipHistory
    {
        public int VacancyOwnerRelationshipHistoryId { get; set; }

        public int VacancyOwnerRelationshipId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime Date { get; set; }

        public int EventTypeId { get; set; }

        public int? EventSubTypeId { get; set; }

        [StringLength(4000)]
        public string Comments { get; set; }

        public virtual VacancyOwnerRelationship VacancyOwnerRelationship { get; set; }

        public virtual VacancyProvisionRelationshipHistoryEventType VacancyProvisionRelationshipHistoryEventType { get; set; }
    }
}
