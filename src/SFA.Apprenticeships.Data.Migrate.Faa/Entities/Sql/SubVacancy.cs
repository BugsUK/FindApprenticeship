namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class SubVacancy
    {
        public int SubVacancyId { get; set; }
        public int VacancyId { get; set; }
        //Technically nullable but for FAA migration purposes will always be set and in fact always is in AVMS
        public int AllocatedApplicationId { get; set; }
        public DateTime? StartDate { get; set; }
        public string ILRNumber { get; set; }
    }
}