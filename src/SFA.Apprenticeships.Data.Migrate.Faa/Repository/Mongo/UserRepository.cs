namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities.Mongo;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using MongoDB.Driver;
    using SFA.Infrastructure.Interfaces;

    public class UserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IConfigurationService configurationService)
        {
            var connectionString = configurationService.Get<MongoConfiguration>().MetricsUsersDb;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public async Task<long> GetUsersCount()
        {
            var cursor = _database.GetCollection<User>("users").CountAsync(Builders<User>.Filter.Empty);
            return await cursor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastId">Pass null or empty for the first page or the Id of the last item in the current page</param>
        public async Task<IAsyncCursor<User>> GetActiveUsersPageAsync(Guid? lastId)
        {
            //http://stackoverflow.com/questions/31675598/how-to-efficiently-page-batches-of-results-with-mongodb

            FilterDefinition<User> filter;
            var filterBuilder = Builders<User>.Filter;
            if (lastId == null || lastId == Guid.Empty)
            {
                filter = filterBuilder.Gte(u => u.Status, 20);
            }
            else
            {
                filter = filterBuilder.Gte(u => u.Status, 20) & filterBuilder.Gt(u => u.Id, lastId.Value);
            }

            var sort = Builders<User>.Sort.Ascending(u => u.Id);
            var options = new FindOptions<User>
            {
                Sort = sort,
                Limit = 500
            };

            var cursor = _database.GetCollection<User>("users").FindAsync(filter, options);
            return await cursor;
        }

        public async Task<long> GetActiveUsersCount()
        {
            var cursor = _database.GetCollection<User>("users").CountAsync(Builders<User>.Filter.Gte(u => u.Status, 20));
            return await cursor;
        }

        public async Task<IDictionary<Guid, User>> GetAllActiveUsersAsync()
        {
            var usersCount = await GetActiveUsersCount();

            var users = new Dictionary<Guid, User>((int)usersCount);

            var lastId = default(Guid?);
            var running = true;

            while (running)
            {
                var cursor = await GetActiveUsersPageAsync(lastId);
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    running = false;
                    foreach (var user in batch)
                    {
                        users[user.Id] = user;
                        lastId = user.Id;
                        running = true;
                    }
                }
            }

            return users;
        }
    }
}