namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;

    public interface IUserMetricsRepository
    {
        long GetRegisteredUserCount();
        long GetRegisteredAndActivatedUserCount();
        long GetActiveUserCount(DateTime activeFrom);
    }
}
