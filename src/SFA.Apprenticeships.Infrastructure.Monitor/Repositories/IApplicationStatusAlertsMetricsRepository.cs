namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    public interface IApplicationStatusAlertsMetricsRepository
    {
        int GetApplicationStatusAlertsProcessedToday();
    }
}
