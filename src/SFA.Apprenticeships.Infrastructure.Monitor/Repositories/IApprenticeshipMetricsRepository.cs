namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using Domain.Entities.Applications;

    public interface IApprenticeshipMetricsRepository
    {
        int GetApplicationCount();
        int GetApplicationStateCount(ApplicationStatuses applicationStatus);
        int GetApplicationCountPerCandidate();
        int GetApplicationStateCountPerCandidate(ApplicationStatuses applicationStatus);
        long GetActiveUserCount(DateTime activeFrom);
        int GetCandidatesWithApplicationsInStatusCount(ApplicationStatuses applicationStatus, int minimumCount);
    }
}