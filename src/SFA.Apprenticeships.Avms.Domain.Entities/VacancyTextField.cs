namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VacancyTextField")]
    public partial class VacancyTextField
    {
        public int VacancyTextFieldId { get; set; }

        public int VacancyId { get; set; }

        public int Field { get; set; }

        public string Value { get; set; }

        public virtual Vacancy Vacancy { get; set; }

        public virtual VacancyTextFieldValue VacancyTextFieldValue { get; set; }
    }
}
