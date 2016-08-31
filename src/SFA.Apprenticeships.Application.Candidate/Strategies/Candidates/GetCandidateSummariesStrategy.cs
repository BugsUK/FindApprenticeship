namespace SFA.Apprenticeships.Application.Candidate.Strategies.Candidates
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class GetCandidateSummariesStrategy : IGetCandidateSummariesStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;

        public GetCandidateSummariesStrategy(ICandidateReadRepository candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
        }

        public IList<CandidateSummary> GetCandidateSummaries(IEnumerable<Guid> candidateIds)
        {
            return _candidateReadRepository.GetCandidateSummaries(candidateIds);
        }
    }
}