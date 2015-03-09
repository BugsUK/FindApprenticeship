namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    public interface IExpiringDraftsMetricsRepository
    {
        int GetDraftApplicationEmailsSentToday();
    }
}
