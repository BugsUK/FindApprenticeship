﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;
    using UserAccount.Strategies;

    public class AuthenticateCandidateStrategy : IAuthenticateCandidateStrategy
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILockAccountStrategy _lockAccountStrategy;
        private readonly IConfigurationManager _configManager;
        private readonly int _maximumPasswordAttemptsAllowed;

        public AuthenticateCandidateStrategy(
            IConfigurationManager configManager,
            IAuthenticationService authenticationService,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ILockAccountStrategy lockAccountStrategy)
        {
            _configManager = configManager;
            _userWriteRepository = userWriteRepository;
            _authenticationService = authenticationService;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _lockAccountStrategy = lockAccountStrategy;
            _maximumPasswordAttemptsAllowed = _configManager.GetAppSetting<int>("MaximumPasswordAttemptsAllowed");
        }

        public Candidate AuthenticateCandidate(string username, string password)
        {
            var user = _userReadRepository.Get(username);

            user.AssertState(
                string.Format("Cannot authenticate user in state: {0}.", user.Status),
                UserStatuses.Active, UserStatuses.PendingActivation, UserStatuses.Locked);

            var authenticated = _authenticationService.AuthenticateUser(user.EntityId, password);

            if (authenticated)
            {
                var candidate = _candidateReadRepository.Get(user.EntityId);

                if (user.Status == UserStatuses.Locked)
                {
                    user.SetStateActive();
                    _userWriteRepository.Save(user);
                }

                //_auditLog.Info(AuditEvents.SuccessfulLogon, username); // TODO: audit successful logon (named logger)

                return candidate;
            }

            RegisterFailedLogin(user);
            // either way, throw an exception to indicate failed auth

            throw new Exception("Authentication failed"); // TODO: EXCEPTION: should use an application exception type
        }

        #region Helpers

        private void RegisterFailedLogin(User user)
        {
            user.LoginIncorrectAttempts++;

            if (user.LoginIncorrectAttempts < _maximumPasswordAttemptsAllowed)
            {
                _userWriteRepository.Save(user);
            }
            else
            {
                _lockAccountStrategy.LockAccount(user);
            }
        }

        #endregion
    }
}
