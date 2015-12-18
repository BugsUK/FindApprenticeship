namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubVacancy")]
    public partial class SubVacancy
    {
        public int SubVacancyId { get; set; }

        public int VacancyId { get; set; }

        public int? AllocatedApplicationId { get; set; }

        public DateTime? StartDate { get; set; }

        [StringLength(12)]
        public string ILRNumber { get; set; }

        public virtual Application Application { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}
