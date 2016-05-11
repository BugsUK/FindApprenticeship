namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class ActivateCandidateStrategy : IActivateCandidateStrategy
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserAccountService _registrationService;

        public ActivateCandidateStrategy(IUserReadRepository userReadRepository, IUserAccountService registrationService)
        {
            _userReadRepository = userReadRepository;
            _registrationService = registrationService;
        }

        public void ActivateCandidate(Guid id, string activationCode)
        {
            var user = _userReadRepository.Get(id);

            user.AssertState("Activate user", UserStatuses.PendingActivation);

            _registrationService.Activate(id, activationCode);
        }
    }
}
