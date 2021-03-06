﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Users;
    using Interfaces;
    using UserAccount.Configuration;
    using UserAccount.Entities;

    public class RegisterCandidateStrategy : IRegisterCandidateStrategy
    {
        private readonly int _activationCodeExpiryDays;
        private readonly IUserAccountService _userAccountService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ICommunicationService _communicationService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IServiceBus _serviceBus;

        public RegisterCandidateStrategy(IConfigurationService configurationService,
            IUserAccountService userAccountService,
            IAuthenticationService authenticationService,
            ICandidateWriteRepository candidateWriteRepository,
            ICommunicationService communicationService,
            ICodeGenerator codeGenerator,
            IUserReadRepository userReadRepository, IServiceBus serviceBus)
        {
            _userAccountService = userAccountService;
            _authenticationService = authenticationService;
            _candidateWriteRepository = candidateWriteRepository;
            _communicationService = communicationService;
            _codeGenerator = codeGenerator;
            _userReadRepository = userReadRepository;
            _serviceBus = serviceBus;
            _activationCodeExpiryDays = configurationService.Get<UserAccountConfiguration>().ActivationCodeExpiryDays;
        }

        public Candidate RegisterCandidate(Candidate newCandidate, string password)
        {
            var username = newCandidate.RegistrationDetails.EmailAddress;
            var activationCode = _codeGenerator.GenerateAlphaNumeric();
            
            var user = _userReadRepository.Get(username, false);

            if (user == null || user.IsInState(UserStatuses.PendingDeletion))
            {
                // Process registration for brand new username
                var newCandidateId = Guid.NewGuid();

                _authenticationService.CreateUser(newCandidateId, password);
                _userAccountService.Register(username, newCandidateId, activationCode, UserRoles.Candidate);

                var createdCandidate = SaveAndNotifyCandidate(newCandidateId, newCandidate, activationCode);
                _serviceBus.PublishMessage(new CandidateUserUpdate(createdCandidate.EntityId, CandidateUserUpdateType.Create));
                return createdCandidate;
            }

            user.AssertState("Register candidate", UserStatuses.PendingActivation);

            if (user.ActivateCodeExpiry != null && user.ActivateCodeExpiry > DateTime.UtcNow)
            {
                // Process existing username in unexpired pending activation status
                return SaveAndNotifyCandidate(user.EntityId, newCandidate, user.ActivationCode);
            }

            // Process existing username in an expired pending activation status
            _authenticationService.ResetUserPassword(user.EntityId, password);
            _userAccountService.Register(username, user.EntityId, activationCode, UserRoles.Candidate);

            var updatedCandidate = SaveAndNotifyCandidate(user.EntityId, newCandidate, activationCode);
            _serviceBus.PublishMessage(new CandidateUserUpdate(updatedCandidate.EntityId, CandidateUserUpdateType.Update));
            return updatedCandidate;
        }

        #region Helpers
        private Candidate SaveAndNotifyCandidate(Guid candidateId, Candidate newCandidate, string activationCode)
        {
            newCandidate.EntityId = candidateId;
            var candidate = _candidateWriteRepository.Save(newCandidate);

            SendActivationCode(candidate, activationCode);

            return candidate;
        }

        private void SendActivationCode(Candidate candidate, string activationCode)
        {
            var emailAddress = candidate.RegistrationDetails.EmailAddress;
            var expiry = FormatActivationCodeExpiryDays();
            var firstName = candidate.RegistrationDetails.FirstName;

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendActivationCode,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, firstName),
                    new CommunicationToken(CommunicationTokens.ActivationCode, activationCode),
                    new CommunicationToken(CommunicationTokens.ActivationCodeExpiryDays, expiry),
                    new CommunicationToken(CommunicationTokens.Username, emailAddress)
                });
        }

        private string FormatActivationCodeExpiryDays()
        {
            return string.Format(_activationCodeExpiryDays == 1 ? "{0} day" : "{0} days", _activationCodeExpiryDays);
        }

        #endregion
    }
}
