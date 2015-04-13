namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using MongoDB.Bson;

    public interface IApprenticeshipMetricsRepository
    {
        int GetApplicationCount();
        int GetApplicationStateCount(ApplicationStatuses applicationStatus);
        int GetApplicationCountPerCandidate();
        int GetApplicationStateCountPerCandidate(ApplicationStatuses applicationStatus);
        long GetActiveUserCount(DateTime activeFrom);
        int GetCandidatesWithApplicationsInStatusCount(ApplicationStatuses applicationStatus, int minimumCount);
        IEnumerable<BsonDocument> GetApplicationStatusCounts();
    }
}