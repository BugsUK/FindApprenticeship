﻿namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Entities.Mongo;
    using MongoDB.Driver;
    using Application.Interfaces;

    public class UserRepository
    {
        private const string CollectionName = "users";

        private readonly ILogService _logService;
        private readonly IMongoDatabase _database;

        public UserRepository(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;
            var connectionString = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>().SourceUsersDb;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public async Task<IAsyncCursor<User>> GetUsersByIds(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            var options = new FindOptions<User>
            {
                Projection = GetUserProjection()
            };
            var filter = Builders<User>.Filter.In(c => c.Id, ids);
            var cursor = _database.GetCollection<User>(CollectionName).FindAsync(filter, options, cancellationToken);
            return await cursor;
        }

        public User GetUser(Guid userGuid)
        {
            var options = new FindOptions<User>
            {
                Projection = GetUserProjection()
            };
            var filter = Builders<User>.Filter.Eq(a => a.Id, userGuid);
            var cursor = _database.GetCollection<User>(CollectionName).FindSync(filter, options);
            return cursor.Single();
        }

        private static ProjectionDefinition<User> GetUserProjection()
        {
            return Builders<User>.Projection
                   .Include(u => u.Id)
                   .Include(u => u.DateCreated)
                   .Include(u => u.DateUpdated)
                   .Include(u => u.Status)
                   .Include(u => u.ActivationDate)
                   .Include(u => u.LastLogin);
        }
    }
}