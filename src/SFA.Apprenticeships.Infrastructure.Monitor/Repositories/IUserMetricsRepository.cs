namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;

    public interface IUserMetricsRepository
    {
        long GetRegisteredUserCount();
        long GetRegisteredUserCount(DateTime dateCreatedStart, DateTime dateCreatedEnd);
        long GetActivatedUserCount();
        long GetActivatedUserCount(DateTime dateActivatedStart, DateTime dateActivatedEnd);
        long GetUnactivatedUserCount();
        long GetUnactivatedExpiredCodeUserCount();
        long GetActiveUserCount(DateTime activeFrom);
        long GetDormantUserCount();
        IEnumerable<BsonDocument> GetUserActivityMetrics(DateTime dateCreatedStart, DateTime dateCreatedEnd);
    }
}
