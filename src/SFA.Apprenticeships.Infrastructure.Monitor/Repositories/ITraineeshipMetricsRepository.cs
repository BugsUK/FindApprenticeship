namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;

    public interface ITraineeshipMetricsRepository
    {
        long GetApplicationCount();
        int GetSubmittedApplicationCount(DateTime submittedDateStart, DateTime submittedDateEnd);
        long GetApplicationsPerCandidateCount();
        IEnumerable<BsonDocument> GetApplicationStatusCounts();
        IEnumerable<BsonDocument> GetApplicationCountPerTraineeship();
        IEnumerable<BsonDocument> GetSubmittedApplicationsCountPerCandidate(DateTime dateCreatedStart, DateTime dateCreatedEnd);
        BsonDocument GetAverageApplicationCountPerTraineeship();
    }
}
