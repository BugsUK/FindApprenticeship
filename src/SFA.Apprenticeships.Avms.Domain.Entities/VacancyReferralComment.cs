namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VacancyReferralComment
    {
        [Key]
        public int VacancyReferralCommentsID { get; set; }

        public int VacancyId { get; set; }

        public int FieldTypeId { get; set; }

        [StringLength(4000)]
        public string Comments { get; set; }

        public virtual Vacancy Vacancy { get; set; }

        public virtual VacancyReferralCommentsFieldType VacancyReferralCommentsFieldType { get; set; }
    }
}
