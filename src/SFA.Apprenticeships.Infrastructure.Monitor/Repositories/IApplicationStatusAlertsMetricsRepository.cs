namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using Domain.Entities.Applications;

    public interface IApplicationStatusAlertsMetricsRepository
    {
        int GetApplicationStatusAlertsProcessedToday();
        int GetApplicationStatusChangedTo(ApplicationStatuses applicationStatus, DateTime statusChangeStartDate, DateTime statusChangeEndDate);
    }
}
