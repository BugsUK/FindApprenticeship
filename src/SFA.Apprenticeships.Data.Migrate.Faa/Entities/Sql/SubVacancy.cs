namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class SubVacancy
    {
        public int SubVacancyId { get; set; }
        public int VacancyId { get; set; }
        public int? AllocatedApplicationId { get; set; }
        public DateTime? StartDate { get; set; }
        public string ILRNumber { get; set; }
    }
}