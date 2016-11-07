namespace SFA.Apprenticeships.Application.Vacancies.Entities
{
    using System;
    using Domain.Entities.Vacancies.Apprenticeships;

    /// <summary>
    /// Adds update reference required for tidy up process
    /// </summary>
    public class ApprenticeshipSummaryUpdate : ApprenticeshipSummary, IVacancyUpdate
    {
        public DateTime ScheduledRefreshDateTime { get; set; }
        public bool UseAlias { get; set; }
    }
}
