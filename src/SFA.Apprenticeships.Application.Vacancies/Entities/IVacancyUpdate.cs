namespace SFA.Apprenticeships.Application.Vacancies.Entities
{
    using System;

    public interface IVacancyUpdate
    {
        DateTime ScheduledRefreshDateTime { get; set; }

        bool UseAlias { get; set; }
    }
}
