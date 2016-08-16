namespace SFA.Apprenticeships.Application.Vacancies.Entities
{
    using System;
    using Domain.Entities.Vacancies.Traineeships;

    /// <summary>
    /// Adds update reference required for tidy up process
    /// </summary>
    public class TraineeshipSummaryUpdate : TraineeshipSummary, IVacancyUpdate
    {
        //todo: 1.8: consider removing this property (and therefore this class)
        public DateTime ScheduledRefreshDateTime { get; set; }
        public bool UseAlias { get; set; }
    }
}
