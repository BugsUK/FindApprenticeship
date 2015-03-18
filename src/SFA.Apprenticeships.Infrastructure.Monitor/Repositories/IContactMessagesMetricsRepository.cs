namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    public interface IContactMessagesMetricsRepository
    {
        int GetContactMessagesSentToday();
    }
}