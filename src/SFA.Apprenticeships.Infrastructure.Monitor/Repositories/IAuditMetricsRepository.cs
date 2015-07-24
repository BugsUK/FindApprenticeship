namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    public interface IAuditMetricsRepository
    {
        long GetAuditCount(string auditEventTypes);
    }
}