﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    public class SavedSearchRepository : GenericMongoClient<MongoSavedSearch>, ISavedSearchReadRepository, ISavedSearchWriteRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public SavedSearchRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger)
            : base(configurationManager, "Candidates.mongoDB", "savedsearches")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public SavedSearch Get(Guid id)
        {
            _logger.Debug("Calling repository to get saved search with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            var messageFormat = mongoEntity == null ? "Found no saved search with Id={0}" : "Found saved search with Id={0}";
            _logger.Debug(messageFormat, id);

            if (mongoEntity == null) return null;

            var entity = _mapper.Map<MongoSavedSearch, SavedSearch>(mongoEntity);

            return entity;
        }

        public IList<SavedSearch> GetForCandidate(Guid candidateId)
        {
            _logger.Debug("Calling repository to get all saved searches for CandidateId={0}", candidateId);

            var mongoEntities = Collection.FindAs<MongoSavedSearch>(Query.EQ("CandidateId", candidateId));
            var entities = _mapper.Map<IEnumerable<MongoSavedSearch>, IEnumerable<SavedSearch>>(mongoEntities).ToList();

            _logger.Debug("Found {0} saved searches for CandidateId={1}", entities.Count, candidateId);

            return entities;
        }

        public IEnumerable<Guid> GetCandidateIds()
        {
            _logger.Debug("Calling repository to get the ids for all candidates that have saved searches with alerts enabled");

            var candidateIds = Collection.AsQueryable().Where(s => s.AlertsEnabled).Select(s => s.CandidateId).Distinct();

            _logger.Debug("Called repository to get the ids for all candidates that have saved searches with alerts enabled");

            return candidateIds;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete saved search with Id={0}", id);

            Collection.Remove(Query<MongoSavedSearch>.EQ(o => o.Id, id));

            _logger.Debug("Deleted saved search with Id={0}", id);
        }

        public SavedSearch Save(SavedSearch entity)
        {
            _logger.Debug("Calling repository to save saved search with Id={0} for CandidateId={1}", entity.EntityId, entity.CandidateId);

            var mongoEntity = _mapper.Map<SavedSearch, MongoSavedSearch>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved saved search to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoSavedSearch, SavedSearch>(mongoEntity);
        }
    }
}
