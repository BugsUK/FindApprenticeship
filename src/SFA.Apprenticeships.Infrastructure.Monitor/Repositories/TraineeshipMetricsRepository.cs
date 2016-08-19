namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Mongo.Candidates.Entities;
    using Infrastructure.Repositories.Mongo.Common;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    using SFA.Apprenticeships.Application.Interfaces;

    public class TraineeshipMetricsRepository : GenericMongoClient<MongoCandidate>, ITraineeshipMetricsRepository
    {
        public TraineeshipMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.MetricsApplicationsDb, "traineeships");
        }

        public long GetApplicationCount()
        {
            return Collection.Count();
        }

        public int GetSubmittedApplicationCount(DateTime submittedDateStart, DateTime submittedDateEnd)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.DateCreated >= submittedDateStart && each.DateCreated < submittedDateEnd);
        }

        public long GetApplicationsPerCandidateCount()
        {
            return Collection.Distinct("CandidateId").Count();
        }

        public IEnumerable<BsonDocument> GetApplicationStatusCounts()
        {
            var group = new BsonDocument
            {
                { 
                    "$group", new BsonDocument
                    {
                        { "_id", "$CandidateId" },
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

        public IEnumerable<BsonDocument> GetApplicationCountPerTraineeship()
        {
            var match = new BsonDocument {{"$match", new BsonDocument {{"Status", new BsonDocument {{"$gte", (int) ApplicationStatuses.Submitted}}}}}};
            var group = new BsonDocument {{"$group", new BsonDocument {{"_id", new BsonDocument {{"VacancyId", "$Vacancy._id"}, {"Title", "$Vacancy.Title"}}}, {"count", new BsonDocument {{"$sum", 1}}}}}};

            var pipeline = new[] { match, group };

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

            var pipeline = new[] { match, @group };
            return pipeline;
        }

        public BsonDocument GetAverageApplicationCountPerTraineeship()
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
                {"$group", new BsonDocument {{"_id", "averages"}, {"traineeshipsWithApplicationsCount", new BsonDocument {{"$sum", 1}}}, {"count", new BsonDocument {{"$sum", "$count"}}}, {"average", new BsonDocument {{"$avg", "$count"}}}}}
            };

            var pipeline = new[] { match, group, average };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result.FirstOrDefault();
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
