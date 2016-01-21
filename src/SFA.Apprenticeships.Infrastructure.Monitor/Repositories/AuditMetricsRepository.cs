namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Linq;
    using Infrastructure.Repositories.Mongo.Audit.Entities;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using SFA.Infrastructure.Interfaces;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;

    public class AuditMetricsRepository : IAuditMetricsRepository
    {
        private const string CollectionName = "auditItems";

        private readonly MongoDatabase _database;
        private MongoCollection<AuditItem> _collection;

        public AuditMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();

            var mongoDbName = MongoUrl.Create(config.AuditDb).DatabaseName;

            _database = new MongoClient(config.MetricsAuditDb).GetServer().GetDatabase(mongoDbName);
        }

        private MongoCollection<AuditItem> Collection
        {
            get
            {
                return _collection ?? (_collection = _database.GetCollection<AuditItem>(CollectionName));
            }
        }

        public long GetAuditCount(string auditEventTypes)
        {
            return Collection
                .AsQueryable()
                .Count(each => each.EventType == auditEventTypes);
        }
    }
}