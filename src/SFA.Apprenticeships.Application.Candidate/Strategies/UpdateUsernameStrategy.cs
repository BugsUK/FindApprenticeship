﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class UpdateUsernameStrategy : IUpdateUsernameStrategy
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;
        private readonly IUserReadRepository _userReadRepository;

        public UpdateUsernameStrategy(IUserAccountService userAccountService,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            ISaveCandidateStrategy saveCandidateStrategy,
            IUserReadRepository userReadRepository)
        {
            _userAccountService = userAccountService;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _saveCandidateStrategy = saveCandidateStrategy;
            _userReadRepository = userReadRepository;
        }

        public void UpdateUsername(Guid userId, string verfiyCode, string password)
        {
            var user = _userAccountService.GetUser(userId);
            var candidate = _candidateReadRepository.Get(userId);

            _userAccountService.UpdateUsername(userId, verfiyCode, password);

            var existingCandidates = _candidateReadRepository.Get(user.PendingUsername, false);
            foreach (var existingCandidate in existingCandidates)
            {
                //Any candidate now associated with a null user record must have been pending activation and can be deleted.
                var existingUser = _userReadRepository.Get(existingCandidate.EntityId);
                if (existingUser == null)
                {
                   _candidateWriteRepository.Delete(existingCandidate.EntityId);
                }
            }

            candidate.RegistrationDetails.EmailAddress = user.PendingUsername;
            _saveCandidateStrategy.SaveCandidate(candidate);
        }
    }
}
