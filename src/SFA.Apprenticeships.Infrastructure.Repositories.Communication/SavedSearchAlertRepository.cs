namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Logging;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;

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
