namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Candidates.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public class CandidateMetricsRepository : GenericMongoClient<MongoCandidate>, ICandidateMetricsRepository
    {
        public CandidateMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.CandidatesDb, "candidates");
        }

        public int GetVerfiedMobileNumbersCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.CommunicationPreferences.AllowMobile);
        }

        public int GetDismissedTraineeshipPromptCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => !each.CommunicationPreferences.AllowTraineeshipPrompts);
        }

        public IEnumerable<Guid> GetCandidatesThatHaveDismissedTheTraineeshipPrompt()
        {
            var match = new BsonDocument {{"$match", new BsonDocument {{"Status", (int) ApplicationStatuses.Submitted}}}};
            var group = new BsonDocument {{"$group", new BsonDocument {{"_id", "_id"}}}};

            var pipeline = new[] { match, group };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result.Select(r => r["_id"].AsGuid);
        }
    }
}