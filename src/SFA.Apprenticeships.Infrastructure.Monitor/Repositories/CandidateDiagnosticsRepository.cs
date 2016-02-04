namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Repositories.Mongo.Candidates.Entities;
    using Infrastructure.Repositories.Mongo.Common;
    using Infrastructure.Repositories.Mongo.Common.Configuration;
    using MongoDB.Driver.Linq;

    public class CandidateDiagnosticsRepository : GenericMongoClient<MongoCandidate, Guid>, ICandidateDiagnosticsRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _userReadRepository;

        public CandidateDiagnosticsRepository(IConfigurationService configurationService, IMapper mapper, IUserReadRepository userReadRepository, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.CandidatesDb, "candidates");
            _mapper = mapper;
            _userReadRepository = userReadRepository;
            _logger = logger;
        }

        public IEnumerable<Candidate> GetActivatedCandidatesWithUnsetLegacyId()
        {
            var activatedCandidatesWithUnsetLegacyId = new List<Candidate>();

            //Message queue back off strategy is to wait 30 seconds before initial retry then 5 minutes for each subsequent retry
            //6 Minutes provides enough time for three attempts
            var outsideLikelyUpdateTime = DateTime.UtcNow.AddMinutes(-6);

            var candidatesWithUnsetLegacyId = Collection.AsQueryable().Where(c => c.DateUpdated < outsideLikelyUpdateTime && c.LegacyCandidateId == 0);
            
            foreach (var mongoCandidate in candidatesWithUnsetLegacyId)
            {
                var user = _userReadRepository.Get(mongoCandidate.EntityId);
                if (user.ActivationCode != null) continue;
                
                var candidate = _mapper.Map<MongoCandidate, Candidate>(mongoCandidate);
                _logger.Debug("Candidate {0} is associated with an activated user but does not have a valid legacy candidate id from the legacy service", candidate.EntityId);
                activatedCandidatesWithUnsetLegacyId.Add(candidate);
            }

            return activatedCandidatesWithUnsetLegacyId;
        }
    }
}