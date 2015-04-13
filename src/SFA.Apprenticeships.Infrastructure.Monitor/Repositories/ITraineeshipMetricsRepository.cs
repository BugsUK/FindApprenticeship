namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Collections.Generic;
    using MongoDB.Bson;

    public interface ITraineeshipMetricsRepository
    {
        long GetApplicationCount();
        long GetApplicationsPerCandidateCount();
        IEnumerable<BsonDocument> GetApplicationStatusCounts();
        IEnumerable<BsonDocument> GetApplicationCountPerTraineeship();
        BsonDocument GetAverageApplicationCountPerTraineeship();
    }
}
