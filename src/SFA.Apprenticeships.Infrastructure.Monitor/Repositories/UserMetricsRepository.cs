﻿namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using MongoUser = Infrastructure.Repositories.Users.Entities.MongoUser;

    public class UserMetricsRepository : GenericMongoClient<MongoUser>, IUserMetricsRepository
    {
        public UserMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.MetricsUsersDb, "users");
        }

        public long GetRegisteredUserCount()
        {
            return Collection.Count();
        }

        public long GetRegisteredUserCount(DateTime dateCreatedStart, DateTime dateCreatedEnd)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.DateCreated >= dateCreatedStart && each.DateCreated < dateCreatedEnd);
        }

        public long GetActivatedUserCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.ActivationCode == null);
        }

        public long GetActivatedUserCount(DateTime dateActivatedStart, DateTime dateActivatedEnd)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.ActivationDate != null && each.ActivationDate >= dateActivatedStart && each.ActivationDate < dateActivatedEnd);
        }

        public long GetUnactivatedUserCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.Status == UserStatuses.PendingActivation);
        }

        public long GetUnactivatedExpiredCodeUserCount()
        {
            var oldestValidCodeDate = DateTime.UtcNow;

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

        public long GetDormantUserCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.Status == UserStatuses.Dormant);
        }

        public IEnumerable<BsonDocument> GetUserActivityMetrics(DateTime dateCreatedStart, DateTime dateCreatedEnd)
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
                        {"_id", new BsonDocument {{"CandidateId", "$_id"}, {"DateCreated", "$DateCreated"}, {"ActivateCodeExpiry", "$ActivateCodeExpiry"}, {"ActivationDate", "$ActivationDate"}, {"LastLogin", "$LastLogin"}}},
                        {"Activated", new BsonDocument {{"$first", new BsonDocument {{"$cond", new BsonArray {new BsonDocument {{"$gte", new BsonArray {"$Status", 20}}}, true, false}}}}}}
                    }
                }
            };

            var pipeline = new[] { match, group };

            var result = Collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return result;
        }
    }
}
