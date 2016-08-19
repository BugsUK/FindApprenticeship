namespace SFA.Apprenticeships.Application.Candidate.Strategies.Candidates
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces;

    public class SearchCandidatesStrategy : ISearchCandidatesStrategy
    {
        private readonly ILogService _logger;
        private readonly ICandidateReadRepository _candidateReadRepository;

        public SearchCandidatesStrategy(ICandidateReadRepository candidateReadRepository, ILogService logger)
        {
            _candidateReadRepository = candidateReadRepository;
            _logger = logger;
        }

        public List<CandidateSummary> SearchCandidates(CandidateSearchRequest request)
        {
            _logger.Debug("Calling CandidateReadRepository to search for candidates that match {0}.", request);

            return _candidateReadRepository.SearchCandidates(request);
        }
    }
}