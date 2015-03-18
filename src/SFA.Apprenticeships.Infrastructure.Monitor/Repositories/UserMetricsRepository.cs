namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Users.Entities;
    using Mongo.Common;
    using MongoDB.Driver.Linq;

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
    }
}
