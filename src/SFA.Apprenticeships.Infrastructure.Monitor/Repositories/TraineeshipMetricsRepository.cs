namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Candidates.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class TraineeshipMetricsRepository : GenericMongoClient<MongoCandidate>, ITraineeshipMetricsRepository
    {
        public TraineeshipMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.ApplicationsDb, "traineeships");
        }

        public long GetApplicationCount()
        {
            return Collection.Count();
        }

        public long GetApplicationsPerCandidateCount()
        {
            return Collection.Distinct("CandidateId").Count();
        }

        public IEnumerable<BsonDocument> GetApplicationStatusCounts()
        {
            var match = new BsonDocument { { "$match", new BsonDocument { { "Status", (int)ApplicationStatuses.Submitted } } } };
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

            var pipeline = new[] { match, group };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result;
        }

        public IEnumerable<BsonDocument> GetApplicationCountPerTraineeship()
        {
            var group = new BsonDocument {{"$group", new BsonDocument {{"_id", new BsonDocument {{"VacancyId", "$Vacancy._id"}, {"Title", "$Vacancy.Title"}}}, {"count", new BsonDocument {{"sum", 1}}}}}};

            var pipeline = new[] { group };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result;
        }

        public BsonDocument GetAverageApplicationCountPerTraineeship()
        {
            var group = new BsonDocument
            {
                {"$group", new BsonDocument {{"_id", new BsonDocument {{"VacancyId", "$Vacancy._id"}, {"Title", "$Vacancy.Title"}}}, {"count", new BsonDocument {{"sum", 1}}}}}
            };
            var average = new BsonDocument
            {
                {"$group", new BsonDocument {{"_id", null}, {"vacanciesWithApplicationsCount", new BsonDocument {{"sum", 1}}}, {"count", new BsonDocument {{"sum", "$count"}}}, {"average", new BsonDocument {{"$avg", "$count"}}}}}
            };

            var pipeline = new[] { group, average };

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
