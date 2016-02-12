namespace SFA.Apprenticeships.Application.Candidate
{
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Interfaces.Candidates;

    public class CandidateSearchService : ICandidateSearchService
    {
        public List<CandidateSummary> SearchCandidates(CandidateSearchRequest request)
        {
            return new List<CandidateSummary>();
        }
    }
}