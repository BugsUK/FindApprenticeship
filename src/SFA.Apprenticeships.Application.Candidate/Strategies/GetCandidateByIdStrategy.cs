namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class GetCandidateByIdStrategy : IGetCandidateByIdStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;

        public GetCandidateByIdStrategy(ICandidateReadRepository candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
        }

        public Candidate GetCandidate(Guid candidateId)
        {
            return _candidateReadRepository.Get(candidateId);
        }
    }
}