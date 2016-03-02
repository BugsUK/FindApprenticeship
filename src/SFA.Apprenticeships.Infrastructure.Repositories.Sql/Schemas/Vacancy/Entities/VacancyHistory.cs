namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.VacancyHistory")]
    public class VacancyHistory
    {
        public int VacancyHistoryId { get; set; }

        public int VacancyId { get; set; }

        public string UserName { get; set; }

        public int VacancyHistoryEventTypeId { get; set; }

        public int? VacancyHistoryEventSubTypeId { get; set; }

        public DateTime HistoryDate { get; set; }

        public string Comment { get; set; }
    }

    public enum VacancyHistoryEventType : int
    {
        StatusChange = 1,
        Note = 2
    }
}
