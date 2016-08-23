namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Audit
{
    using System;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Common.Configuration;
    using MongoDB.Driver;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class AuditRepository : IAuditRepository
    {
        private const string CollectionName = "auditItems";

        private readonly ILogService _logger;
        private readonly MongoDatabase _database;
        private MongoCollection<AuditItem> _collection;

        public AuditRepository(IConfigurationService configurationService, ILogService logger)
        {
            _logger = logger;

            var config = configurationService.Get<MongoConfiguration>();

            var mongoDbName = MongoUrl.Create(config.AuditDb).DatabaseName;

            _database = new MongoClient(config.AuditDb).GetServer().GetDatabase(mongoDbName);
        }

        private MongoCollection<AuditItem> Collection
        {
            get
            {
                return _collection ?? (_collection = _database.GetCollection<AuditItem>(CollectionName));
            }
        }

        public void Audit(object data, string eventType, Guid primaryEntityId, Guid? secondaryEntityId = null)
        {
            var auditItem = new AuditItem
            {
                Id = Guid.NewGuid(),
                EventDate = DateTime.UtcNow,
                EventType = eventType,
                PrimaryEntityId = primaryEntityId,
                SecondaryEntityId = secondaryEntityId,
                Data = data
            };

            _logger.Debug("Called Mongodb to save audit item for data with PrimaryEntityId={0}, SecondaryEntityId={1}", primaryEntityId, secondaryEntityId);

            Collection.Save(auditItem);

            _logger.Debug("Saved audit item for data with PrimaryEntityId={0}, SecondaryEntityId={1}", primaryEntityId, secondaryEntityId);
        }
    }
}