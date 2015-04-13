namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Mongo.Common;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using MongoUser = Infrastructure.Repositories.Users.Entities.MongoUser;

    public class UserMetricsRepository : GenericMongoClient<MongoUser>, IUserMetricsRepository
    {
        public UserMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Users.mongoDB", "users")
        {
        }

        public long GetRegisteredUserCount()
        {
            return Collection.Count();
        }

        public long GetRegisteredAndActivatedUserCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.ActivationCode == null);
        }

        public long GetUnactivatedUserCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.Status == UserStatuses.PendingActivation);
        }

        public long GetUnactivatedExpiredCodeUserCount()
        {
            var oldestValidCodeDate = DateTime.UtcNow.AddDays(-30);

            return Collection
                .AsQueryable()
                .Count(each => each.Status == UserStatuses.PendingActivation && each.ActivateCodeExpiry < oldestValidCodeDate);
        }

        public long GetActiveUserCount(DateTime activeFrom)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.LastLogin != null && each.LastLogin >= activeFrom);
        }

        public IEnumerable<BsonDocument> GetUserActivityMetrics()
        {
            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        {"_id", new BsonDocument {{"CandidateId", "$_id"}, {"DateCreated", "$DateCreated"}, {"ActivateCodeExpiry", "$ActivateCodeExpiry"}, {"ActivationDate", "$ActivationDate"}, {"LastLogin", "$LastLogin"}}},
                        {"Activated", new BsonDocument {{"$first", new BsonDocument {{"$cond", new BsonArray {new BsonDocument {{"$gte", new BsonArray {"$Status", 20}}}, true, false}}}}}}
                    }
                }
            };

            var pipeline = new[] { group };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result;
        }
    }
}
