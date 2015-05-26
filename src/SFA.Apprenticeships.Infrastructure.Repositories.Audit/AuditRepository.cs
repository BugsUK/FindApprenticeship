﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Audit
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common.Configuration;
    using MongoDB.Driver;

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