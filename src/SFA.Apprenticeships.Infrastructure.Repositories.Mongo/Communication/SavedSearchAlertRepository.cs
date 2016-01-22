namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Repositories;
    using Mongo.Communication.Entities;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using SFA.Infrastructure.Interfaces;

    public class SavedSearchAlertRepository : CommunicationRepository<SavedSearchAlert>, ISavedSearchAlertRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public SavedSearchAlertRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
            : base(configurationService, "savedsearchalerts")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public SavedSearchAlert Get(Guid id)
        {
            _logger.Debug("Calling repository to get saved search alert with Id={0}", id);

            var mongoEntity = Collection.FindOneByIdAs<MongoSavedSearchAlert>(id);
            var message = mongoEntity == null ? "Found no saved search alert with Id={0}" : "Found saved search alert with Id={0}";

            _logger.Debug(message, id);

            return _mapper.Map<MongoSavedSearchAlert, SavedSearchAlert>(mongoEntity);
        }

        public SavedSearchAlert GetUnsentSavedSearchAlert(SavedSearch savedSearch)
        {
            _logger.Debug("Calling repository to get unsent saved search alert for saved search Id={0}",
                savedSearch.EntityId);

            var mongoAlert =
                Collection.AsQueryable<MongoSavedSearchAlert>()
                    .SingleOrDefault(a => a.BatchId == null && a.Parameters.EntityId == savedSearch.EntityId);

            if (mongoAlert == null)
            {
                _logger.Debug("Did not find unsent saved search alert for saved search Id={0}", savedSearch.EntityId);
                return null;
            }

            _logger.Debug("Found unsent saved search alert for saved search Id={0}", savedSearch.EntityId);
            var alert = _mapper.Map<MongoSavedSearchAlert, SavedSearchAlert>(mongoAlert);
            return alert;
        }

        public void Save(SavedSearchAlert savedSearchAlert)
        {
            _logger.Debug(
                "Calling repository to save saved search alert with Id={0} for saved search with Id={1} and CandidateId={2}",
                savedSearchAlert.EntityId, savedSearchAlert.Parameters.EntityId, savedSearchAlert.Parameters.CandidateId);

            var mongoAlert = _mapper.Map<SavedSearchAlert, MongoSavedSearchAlert>(savedSearchAlert);
            UpdateEntityTimestamps(mongoAlert);
            mongoAlert.SentDateTime = mongoAlert.BatchId.HasValue ? mongoAlert.DateUpdated : null;

            Collection.Save(mongoAlert);

            _logger.Debug("Saved saved search alert with Id={0} for saved search with Id={1} and CandidateId={2}",
                savedSearchAlert.EntityId, savedSearchAlert.Parameters.EntityId, savedSearchAlert.Parameters.CandidateId);
        }

        public void Delete(SavedSearchAlert savedSearchAlert)
        {
            _logger.Debug("Calling repository to delete saved search alert with Id={0}", savedSearchAlert.EntityId);

            Collection.Remove(Query.EQ("_id", savedSearchAlert.EntityId));

            _logger.Debug("Deleted saved search alert with Id={0}", savedSearchAlert.EntityId);
        }

        public Dictionary<Guid, List<SavedSearchAlert>> GetCandidatesSavedSearchAlerts()
        {
            _logger.Debug("Calling repository to get all saved search alerts");

            var mongoSavedSearchAlerts = Collection.FindAs<MongoSavedSearchAlert>(Query.EQ("BatchId", BsonNull.Value));
            var savedSearchAlerts = _mapper.Map<IEnumerable<MongoSavedSearchAlert>, IEnumerable<SavedSearchAlert>>(mongoSavedSearchAlerts);
            var candidatesSavedSearchAlerts = savedSearchAlerts
                .GroupBy(x => x.Parameters.CandidateId)
                .ToDictionary(grp => grp.Key, grp => grp.ToList());

            _logger.Debug("Found saved search alerts for {0} candidates", candidatesSavedSearchAlerts.Count);

            return candidatesSavedSearchAlerts;
        }

        public IEnumerable<Guid> GetAlertsCreatedOnOrBefore(DateTime dateTime)
        {
            _logger.Debug("Calling repository to get all saved search alerts created on or before={0}", dateTime);

            var alertIds = Collection
                .AsQueryable<MongoSavedSearchAlert>()
                .Where(each => each.DateCreated <= dateTime)
                .Select(each => each.EntityId);

            _logger.Debug("Called repository to get all saved search alerts created on or before={0}", dateTime);

            return alertIds;
        }
    }
}