namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Applications.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public class ApprenticeshipMetricsRepository : GenericMongoClient<MongoApprenticeshipApplicationDetail>, IApprenticeshipMetricsRepository
    {
        public ApprenticeshipMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.ApplicationsDb, "apprenticeships");
        }

        public int GetApplicationCount()
        {
            return (int)Collection.Count();
        }

        public int GetApplicationStateCount(ApplicationStatuses applicationStatus)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.Status == applicationStatus);
        }

        public int GetApplicationCountPerCandidate()
        {
            return Collection.Distinct("CandidateId").Count();
        }

        public int GetApplicationStateCountPerCandidate(ApplicationStatuses applicationStatus)
        {
            return Collection
                .AsQueryable()
                .Where(each => each.Status == applicationStatus)
                .Select(each => each.CandidateId)
                .Distinct()
                .ToList()
                .Count;
        }

        public long GetActiveUserCount(DateTime activeFrom)
        {
            return Collection
                .AsQueryable()
                .Where(each => each.DateCreated >= activeFrom || each.DateUpdated >= activeFrom)
                .Select(each => each.CandidateId)
                .Distinct()
                .ToList()
                .Count;
        }

        public int GetCandidatesWithApplicationsInStatusCount(ApplicationStatuses applicationStatus, int minimumCount)
        {
            var statusMatch = new BsonDocument {{"$match", new BsonDocument {{"Status", (int) applicationStatus}}}};
            var candidateIdGroup = new BsonDocument {{"$group", new BsonDocument {{"_id", "$CandidateId"}, {"count", new BsonDocument{{"$sum", 1}}}}}};
            var countMatch = new BsonDocument {{"$match", new BsonDocument {{"count", new BsonDocument {{"$gte", minimumCount}}}}}};
            var sumGroup = new BsonDocument {{"$group", new BsonDocument {{"_id", "candidatesWithApplicationsInStatusCount"}, {"candidatesWithApplicationsInStatusCount", new BsonDocument {{"$sum", 1}}}}}};

            var pipeline = new[] { statusMatch, candidateIdGroup, countMatch, sumGroup };

            var result = Collection.Aggregate(new AggregateArgs{Pipeline = pipeline}).ToArray();
            var count = result.Length == 0 ? 0 : result[0].ToArray()[1].Value.AsInt32;

            return count;
        }
    }
}
