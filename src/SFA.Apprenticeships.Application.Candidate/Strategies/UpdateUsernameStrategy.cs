namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class UpdateUsernameStrategy : IUpdateUsernameStrategy
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;

        public UpdateUsernameStrategy(IUserAccountService userAccountService,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            ISaveCandidateStrategy saveCandidateStrategy)
        {
            _userAccountService = userAccountService;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _saveCandidateStrategy = saveCandidateStrategy;
        }

        public void UpdateUsername(Guid userId, string verfiyCode, string password)
        {
            var user = _userAccountService.GetUser(userId);
            var candidate = _candidateReadRepository.Get(userId);

            _userAccountService.UpdateUsername(userId, verfiyCode, password);

            //Updating user succeeded, therefore any candidates with the pending username must have been pending activation and can be deleted.
            var pendingCandidate = _candidateReadRepository.Get(user.PendingUsername, false);
            if (pendingCandidate != null)
            {
                _candidateWriteRepository.Delete(pendingCandidate.EntityId);
            }

            candidate.RegistrationDetails.EmailAddress = user.Username;
            _saveCandidateStrategy.SaveCandidate(candidate);
        }
    }
}
