namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    public interface ISavedSearchAlertMetricsRepository
    {
        int GetSavedSearchAlertsProcessedToday();
    }
}