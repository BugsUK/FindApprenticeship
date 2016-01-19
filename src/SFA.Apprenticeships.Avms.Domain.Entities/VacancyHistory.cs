namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VacancyHistory")]
    public partial class VacancyHistory
    {
        public int VacancyHistoryId { get; set; }

        public int VacancyId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        public int VacancyHistoryEventTypeId { get; set; }

        public int? VacancyHistoryEventSubTypeId { get; set; }

        public DateTime HistoryDate { get; set; }

        [StringLength(4000)]
        public string Comment { get; set; }

        public virtual Vacancy Vacancy { get; set; }

        public virtual VacancyHistoryEventType VacancyHistoryEventType { get; set; }
    }
}
