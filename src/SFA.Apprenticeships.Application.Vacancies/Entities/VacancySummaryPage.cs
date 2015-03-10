namespace SFA.Apprenticeships.Application.Vacancies.Entities
{
    using System;

    public class VacancySummaryPage
    {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public DateTime ScheduledRefreshDateTime { get; set; }
    }
}