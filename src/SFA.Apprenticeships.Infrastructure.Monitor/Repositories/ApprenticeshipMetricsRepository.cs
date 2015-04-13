namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Applications.Entities;
    using Mongo.Common;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public class ApprenticeshipMetricsRepository : GenericMongoClient<MongoApprenticeshipApplicationDetail>, IApprenticeshipMetricsRepository
    {
        public ApprenticeshipMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Applications.mongoDB", "apprenticeships")
        {
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

        public IEnumerable<BsonDocument> GetApplicationStatusCounts()
        {
            var group = new BsonDocument
            {
                { 
                    "$group", new BsonDocument
                    {
                        { "_id", "$CandidateId" },
                        { "Saved", GetApplicationStatusCount(ApplicationStatuses.Saved) },
                        { "Draft", GetApplicationStatusCount(ApplicationStatuses.Draft) },
                        { "Submitted", GetApplicationStatusCount(ApplicationStatuses.Submitted) },
                        { "Unsuccessful", GetApplicationStatusCount(ApplicationStatuses.Unsuccessful) },
                        { "Successful", GetApplicationStatusCount(ApplicationStatuses.Successful) }
                    } 
                }
            };

            var pipeline = new[] { group };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result;
        }

        public IEnumerable<Guid> GetCandidatesThatWouldHaveSeenTraineeshipPrompt()
        {
            var statusMatch = new BsonDocument {{"$match", new BsonDocument {{"Status", (int) ApplicationStatuses.Unsuccessful}}}};
            var candidateIdGroup = new BsonDocument {{"$group", new BsonDocument {{"_id", "$CandidateId"}, {"count", new BsonDocument {{"$sum", 1}}}}}};
            var countMatch = new BsonDocument {{"$match", new BsonDocument {{"count", new BsonDocument {{"$gte", 6}}}}}};
           
            var pipeline = new[] { statusMatch, candidateIdGroup, countMatch };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result.Select(r => r["_id"].AsGuid);
        }

        public IEnumerable<BsonDocument> GetApplicationCountPerApprenticeship()
        {
            var match = new BsonDocument {{"$match", new BsonDocument {{"Status", new BsonDocument {{"$gte", (int) ApplicationStatuses.Submitted}}}}}};
            var group = new BsonDocument {{"$group", new BsonDocument {{"_id", new BsonDocument {{"VacancyId", "$Vacancy._id"}, {"Title", "$Vacancy.Title"}}}, {"count", new BsonDocument {{"$sum", 1}}}}}};

            var pipeline = new[] { match, group };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result;
        }

        public BsonDocument GetAverageApplicationCountPerApprenticeship()
        {
            var match = new BsonDocument
            {
                {"$match", new BsonDocument {{"Status", new BsonDocument {{"$gte", (int) ApplicationStatuses.Submitted}}}}}
            };
            var group = new BsonDocument
            {
                {"$group", new BsonDocument {{"_id", new BsonDocument {{"VacancyId", "$Vacancy._id"}, {"Title", "$Vacancy.Title"}}}, {"count", new BsonDocument {{"$sum", 1}}}}}
            };
            var average = new BsonDocument
            {
                {"$group", new BsonDocument {{"_id", "averages"}, {"apprenticeshipsWithApplicationsCount", new BsonDocument {{"$sum", 1}}}, {"count", new BsonDocument {{"$sum", "$count"}}}, {"average", new BsonDocument {{"$avg", "$count"}}}}}
            };

            var pipeline = new[] { match, group, average };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result.First();
        }

        private static BsonDocument GetApplicationStatusCount(ApplicationStatuses applicationStatus)
        {
            return new BsonDocument
            {
                {
                    "$sum", new BsonDocument
                    {
                        {
                            "$cond", new BsonArray
                            {
                                new BsonDocument {{"$eq", new BsonArray {"$Status", (int) applicationStatus}}}, 1, 0
                            }
                        }
                    }
                }
            };
        }
    }
}
