namespace SFA.Apprenticeships.Infrastructure.Repositories.Audit
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
        private MongoCollection<AuditItem<User>> _userCollection;
        private MongoCollection<AuditItem<Candidate>> _candidateCollection;

        public AuditRepository(IConfigurationService configurationService, ILogService logger)
        {
            _logger = logger;

            var config = configurationService.Get<MongoConfiguration>();

            var mongoDbName = MongoUrl.Create(config.AuditDb).DatabaseName;

            _database = new MongoClient(config.AuditDb).GetServer().GetDatabase(mongoDbName);
        }

        private MongoCollection<AuditItem<User>> UserCollection
        {
            get
            {
                return _userCollection ?? (_userCollection = _database.GetCollection<AuditItem<User>>(CollectionName));
            }
        }

        private MongoCollection<AuditItem<Candidate>> CandidateCollection
        {
            get
            {
                return _candidateCollection ?? (_candidateCollection = _database.GetCollection<AuditItem<Candidate>>(CollectionName));
            }
        }

        public void Audit(User user, string eventType)
        {
            var auditItem = new AuditItem<User>
            {
                Id = Guid.NewGuid(),
                EventDate = DateTime.UtcNow,
                EventType = eventType,
                PrimaryEntityId = user.EntityId,
                Data = user
            };

            _logger.Debug("Called Mongodb to save audit item for user with Id={0}", user.EntityId);

            UserCollection.Save(auditItem);

            _logger.Debug("Saved audit item for user with Id={0}", user.EntityId);
        }

        public void Audit(Candidate candidate, string eventType)
        {
            var auditItem = new AuditItem<Candidate>
            {
                Id = Guid.NewGuid(),
                EventDate = DateTime.UtcNow,
                EventType = eventType,
                PrimaryEntityId = candidate.EntityId,
                Data = candidate
            };

            _logger.Debug("Called Mongodb to save audit item for candidate with Id={0}", candidate.EntityId);

            CandidateCollection.Save(auditItem);

            _logger.Debug("Saved audit item for candidate with Id={0}", candidate.EntityId);
        }
    }
}