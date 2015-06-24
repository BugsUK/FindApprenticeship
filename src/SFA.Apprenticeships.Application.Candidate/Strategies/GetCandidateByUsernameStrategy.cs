namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;

    public class GetCandidateByUsernameStrategy : IGetCandidateByUsernameStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;

        public GetCandidateByUsernameStrategy(ICandidateReadRepository candidateReadRepository, IUserReadRepository userReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
        }

        public Candidate GetCandidate(string username)
        {
            var candidates = _candidateReadRepository.Get(username).ToList();

            if (candidates.Count == 1)
            {
                return candidates.Single();
            }

            var user = _userReadRepository.Get(username);
            return _candidateReadRepository.Get(user.EntityId);
        }
    }
}