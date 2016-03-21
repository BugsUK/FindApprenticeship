namespace SFA.Apprenticeships.Application.Candidate
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Candidates;

    public class CandidateSearchService : ICandidateSearchService
    {
        private readonly ILogService _logger;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public CandidateSearchService(ICandidateReadRepository candidateReadRepository, ILogService logger)
        {
            _candidateReadRepository = candidateReadRepository;
            _logger = logger;
        }

        public List<CandidateSummary> SearchCandidates(CandidateSearchRequest request)
        {
            Condition.Requires(request);

            _logger.Debug("Calling CandidateReadRepository to search for candidates that match {0}.", request);

            return _candidateReadRepository.SearchCandidates(request);
        }
    }
}