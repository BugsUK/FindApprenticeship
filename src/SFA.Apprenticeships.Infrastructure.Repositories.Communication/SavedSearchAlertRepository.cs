namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
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

            var alert = Collection.AsQueryable().SingleOrDefault(a => a.BatchId == null && a.Parameters.EntityId == savedSearch.EntityId);

            if (alert == null)
            {
                _logger.Debug("Did not find unsent saved search alert for saved search Id={0}", savedSearch.EntityId);
            }
            else
            {
                _logger.Debug("Found unsent saved search alert for saved search Id={0}", savedSearch.EntityId);
            }

            return alert;
        }

        public void Save(SavedSearchAlert savedSearchAlert)
        {
            throw new NotImplementedException();
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
