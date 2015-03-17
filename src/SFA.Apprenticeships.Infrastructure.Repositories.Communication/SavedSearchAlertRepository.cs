﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using MongoDB.Driver.Linq;

    //todo: 1.8: comms repo for saved search alerts
    public class SavedSearchAlertRepository : CommunicationRepository<SavedSearchAlert>, ISavedSearchAlertRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public SavedSearchAlertRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger) : base(configurationManager, "savedsearchalerts")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public SavedSearchAlert GetUnsentSavedSearchAlert(SavedSearch savedSearch)
        {
            _logger.Debug("Calling repository to get unsent saved search alert for saved search Id={0}", savedSearch.EntityId);

            var mongoAlert = Collection.AsQueryable<MongoSavedSearchAlert>().SingleOrDefault(a => a.BatchId == null && a.Parameters.EntityId == savedSearch.EntityId);

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
            _logger.Debug("Calling repository to save saved search alert with Id={0} for saved search with Id={1} and CandidateId={2}", savedSearchAlert.EntityId, savedSearchAlert.Parameters.EntityId, savedSearchAlert.Parameters.CandidateId);

            var mongoAlert = _mapper.Map<SavedSearchAlert, MongoSavedSearchAlert>(savedSearchAlert);
            UpdateEntityTimestamps(mongoAlert);
            mongoAlert.SentDateTime = mongoAlert.BatchId.HasValue ? mongoAlert.DateUpdated : null;

            Collection.Save(mongoAlert);

            _logger.Debug("Saved saved search alert with Id={0} for saved search with Id={1} and CandidateId={2}", savedSearchAlert.EntityId, savedSearchAlert.Parameters.EntityId, savedSearchAlert.Parameters.CandidateId);
        }

        public void Delete(SavedSearchAlert savedSearchAlert)
        {
            throw new NotImplementedException();
        }

        public Dictionary<Guid, List<SavedSearchAlert>> GetCandidatesSavedSearchAlerts()
        {
            throw new NotImplementedException();
        }
    }
}
