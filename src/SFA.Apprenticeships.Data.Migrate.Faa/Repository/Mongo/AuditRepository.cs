namespace SFA.Apprenticeships.Data.Migrate.Faa.Repository.Mongo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Configuration;
    using Entities.Mongo;
    using MongoDB.Driver;

    public class AuditRepository
    {
        private const string CollectionName = "auditItems";

        private static readonly string[] RelevantAuditEventTypes = {
            AuditEventTypes.HardDeleteCandidateUser
        };

        private readonly ILogService _logService;
        private readonly IMongoDatabase _database;

        public AuditRepository(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;
            var connectionString = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>().SourceAuditDb;
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public async Task<long> GetAuditItemsCount(CancellationToken cancellationToken)
        {
            var filter = Builders<AuditItem>.Filter.In(a => a.EventType, RelevantAuditEventTypes);
            var cursor = _database.GetCollection<AuditItem>(CollectionName).CountAsync(filter, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<long> GetAuditItemsCreatedSinceCount(DateTime lastEventDate, CancellationToken cancellationToken)
        {
            var filter = Builders<AuditItem>.Filter.In(a => a.EventType, RelevantAuditEventTypes) & Builders<AuditItem>.Filter.Gt(a => a.EventDate, lastEventDate);
            var cursor = _database.GetCollection<AuditItem>(CollectionName).CountAsync(filter, cancellationToken: cancellationToken);
            return await cursor;
        }

        public async Task<IAsyncCursor<AuditItem>> GetAllAuditItems(CancellationToken cancellationToken)
        {
            var filter = Builders<AuditItem>.Filter.In(a => a.EventType, RelevantAuditEventTypes);
            var sort = Builders<AuditItem>.Sort.Ascending(a => a.EventDate);
            var options = new FindOptions<AuditItem>
            {
                Sort = sort,
                BatchSize = 2000,
                Projection = GetAuditItemProjection()
            };
            return await _database.GetCollection<AuditItem>(CollectionName).FindAsync(filter, options, cancellationToken);
        }

        public async Task<IAsyncCursor<AuditItem>> GetAllAuditItemsCreatedSince(DateTime lastEventDate, CancellationToken cancellationToken)
        {
            var filter = Builders<AuditItem>.Filter.In(a => a.EventType, RelevantAuditEventTypes) & Builders<AuditItem>.Filter.Gt(a => a.EventDate, lastEventDate);
            var sort = Builders<AuditItem>.Sort.Ascending(a => a.EventDate);
            var options = new FindOptions<AuditItem>
            {
                Sort = sort,
                BatchSize = 2000,
                Projection = GetAuditItemProjection()
            };
            return await _database.GetCollection<AuditItem>(CollectionName).FindAsync(filter, options, cancellationToken);
        }

        private static ProjectionDefinition<AuditItem> GetAuditItemProjection()
        {
            return Builders<AuditItem>.Projection
                .Include(c => c.Id)
                .Include(c => c.EventDate)
                .Include(c => c.EventType)
                .Include(c => c.PrimaryEntityId)
                .Include(c => c.SecondaryEntityId);
        }
    }
}