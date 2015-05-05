namespace SFA.Apprenticeships.Application.Vacancy.SiteMap
{
    using System;
    using Domain.Entities.Vacancies;

    public class SiteMapVacancy
    {
        public int VacancyId { get; set; }

        public VacancyType VacancyType { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
