namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class UpdateUsernameStrategy : IUpdateUsernameStrategy
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;

        public UpdateUsernameStrategy(IUserAccountService userAccountService,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository)
        {
            _userAccountService = userAccountService;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        public void UpdateUsername(Guid userId, string verfiyCode, string password)
        {
            _userAccountService.UpdateUsername(userId, verfiyCode, password);

            var user = _userAccountService.GetUser(userId);
            var candidate = _candidateReadRepository.Get(userId);
            candidate.RegistrationDetails.EmailAddress = user.Username;
            _candidateWriteRepository.Save(candidate);

            var apprenticeshipApplications = _apprenticeshipApplicationReadRepository.GetForCandidate(userId);

            foreach (var apprenticeshipApplicationSummary in apprenticeshipApplications.Where(a => a.Status == ApplicationStatuses.Draft || a.Status == ApplicationStatuses.Saved))
            {
                var application = _apprenticeshipApplicationReadRepository.Get(apprenticeshipApplicationSummary.ApplicationId);
                application.CandidateDetails.EmailAddress = user.Username;
                _apprenticeshipApplicationWriteRepository.Save(application);
            }
        }
    }
}
