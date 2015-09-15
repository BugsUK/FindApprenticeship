namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
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
            Initialise(config.MetricsApplicationsDb, "apprenticeships");
        }

        public int GetApplicationCount()
        {
            return (int)Collection.Count();
        }

        public int GetSubmittedApplicationCount(DateTime submittedDateStart, DateTime submittedDateEnd)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.DateApplied >= submittedDateStart && each.DateApplied < submittedDateEnd);
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

        public IEnumerable<BsonDocument> GetUnsubmittedApplicationsCountPerCandidate(DateTime dateCreatedStart, DateTime dateCreatedEnd)
        {
            var pipeline = GetApplicationsCountPerCandidatePipeline(dateCreatedStart, dateCreatedEnd, "$lt");

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result;
        }

        public IEnumerable<BsonDocument> GetSubmittedApplicationsCountPerCandidate(DateTime dateCreatedStart, DateTime dateCreatedEnd)
        {
            var pipeline = GetApplicationsCountPerCandidatePipeline(dateCreatedStart, dateCreatedEnd, "$gte");

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result;
        }

        private static BsonDocument[] GetApplicationsCountPerCandidatePipeline(DateTime dateCreatedStart, DateTime dateCreatedEnd, string modifier)
        {
            var match = new BsonDocument
            {
                {
                    "$match",
                    new BsonDocument
                    {
                        {
                            "$and",
                            new BsonArray
                            {
                                new BsonDocument {{"Status", new BsonDocument {{modifier, (int) ApplicationStatuses.Submitted}}}},
                                new BsonDocument {{"DateCreated", new BsonDocument {{"$gte", dateCreatedStart}}}},
                                new BsonDocument {{"DateCreated", new BsonDocument {{"$lte", dateCreatedEnd}}}}
                            }
                        }
                    }
                }
            };
            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        {"_id", new BsonDocument {{"CandidateId", "$CandidateId"}}},
                        {"count", new BsonDocument {{"$sum", 1}}}
                    }
                }
            };

            var pipeline = new[] {match, @group};
            return pipeline;
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
