﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class UpdateUsernameStrategy : IUpdateUsernameStrategy
    {
        private readonly IUserAccountService _userAccountService;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;

        public UpdateUsernameStrategy(IUserAccountService userAccountService,
            ICandidateReadRepository candidateReadRepository,
            ISaveCandidateStrategy saveCandidateStrategy)
        {
            _userAccountService = userAccountService;
            _candidateReadRepository = candidateReadRepository;
            _saveCandidateStrategy = saveCandidateStrategy;
        }

        public void UpdateUsername(Guid userId, string verfiyCode, string password)
        {
            _userAccountService.UpdateUsername(userId, verfiyCode, password);
            var user = _userAccountService.GetUser(userId);
            var candidate = _candidateReadRepository.Get(userId);
            candidate.RegistrationDetails.EmailAddress = user.Username;
            _saveCandidateStrategy.SaveCandidate(candidate);
        }
    }
}
