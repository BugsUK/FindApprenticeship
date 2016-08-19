namespace SFA.Apprenticeships.Application.Candidate
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Candidates;
    using Strategies.Candidates;
    
    public class CandidateSearchService : ICandidateSearchService
    {
        private readonly ISearchCandidatesStrategy _searchCandidatesStrategy;

        public CandidateSearchService(ISearchCandidatesStrategy searchCandidatesStrategy)
        {
            _searchCandidatesStrategy = searchCandidatesStrategy;
        }

        public List<CandidateSummary> SearchCandidates(CandidateSearchRequest request)
        {
            Condition.Requires(request);

            return _searchCandidatesStrategy.SearchCandidates(request);
        }
    }
}