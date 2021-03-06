namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces;
    using Interfaces.Communications;
    using Interfaces.Users;
    using System;
    using Domain.Interfaces.Messaging;
    using Entities;

    public class ResetForgottenPasswordStrategy : IResetForgottenPasswordStrategy
    {
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ICommunicationService _communicationService;
        private readonly ILockAccountStrategy _lockAccountStrategy;
        private readonly IAuthenticationService _authenticationService;
        private readonly IServiceBus _serviceBus;
        private readonly int _maximumPasswordAttemptsAllowed;

        public ResetForgottenPasswordStrategy(ICommunicationService communicationService,
            ILockAccountStrategy lockAccountStrategy,
            ICandidateReadRepository candidateReadRepository,
            IUserReadRepository userReadRepository,
            IAuthenticationService authenticationService,
            IConfigurationService configurationService,
            IUserWriteRepository userWriteRepository,
            IAuditRepository auditRepository, IServiceBus serviceBus)
        {
            _communicationService = communicationService;
            _lockAccountStrategy = lockAccountStrategy;
            _candidateReadRepository = candidateReadRepository;
            _userReadRepository = userReadRepository;
            _authenticationService = authenticationService;
            _userWriteRepository = userWriteRepository;
            _auditRepository = auditRepository;
            _serviceBus = serviceBus;
            _maximumPasswordAttemptsAllowed = configurationService.Get<UserAccountConfiguration>().MaximumPasswordAttemptsAllowed;
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            var user = _userReadRepository.Get(username);

            var candidate = _candidateReadRepository.Get(user.EntityId);

            if (user.PasswordResetCode != null && user.PasswordResetCode.Equals(passwordCode, StringComparison.CurrentCultureIgnoreCase))
            {
                if (user.PasswordResetCodeExpiry != null && DateTime.UtcNow > user.PasswordResetCodeExpiry)
                {
                    throw new CustomException("Password reset code has expired.", Interfaces.Users.ErrorCodes.UserPasswordResetCodeExpiredError);
                }

                _authenticationService.ResetUserPassword(user.EntityId, newPassword);

                user.SetStateActive();

                user.LastLogin = DateTime.UtcNow;

                _userWriteRepository.Save(user);
                _serviceBus.PublishMessage(new CandidateUserUpdate(user.EntityId, CandidateUserUpdateType.Update));
                _auditRepository.Audit(user, AuditEventTypes.UserResetPassword, user.EntityId);

                SendPasswordResetConfirmationMessage(candidate);
            }
            else
            {
                RegisterFailedPasswordReset(user);

                throw new CustomException("Password reset code \"{0}\" is invalid for user \"{1}\"", Interfaces.Users.ErrorCodes.UserPasswordResetCodeIsInvalid, passwordCode, username);
            }
        }

        #region Helpers

        private void RegisterFailedPasswordReset(User user)
        {
            if (user.PasswordResetIncorrectAttempts == _maximumPasswordAttemptsAllowed)
            {
                _lockAccountStrategy.LockAccount(user);
                _serviceBus.PublishMessage(new CandidateUserUpdate(user.EntityId, CandidateUserUpdateType.Update));
                throw new CustomException("Maximum password attempts allowed reached, account is now locked.", Interfaces.Users.ErrorCodes.UserAccountLockedError);
            }

            user.PasswordResetIncorrectAttempts++;
            _userWriteRepository.Save(user);
            _serviceBus.PublishMessage(new CandidateUserUpdate(user.EntityId, CandidateUserUpdateType.Update));
        }

        private void SendPasswordResetConfirmationMessage(Candidate candidate)
        {
            var firstName = candidate.RegistrationDetails.FirstName;
            var emailAddress = candidate.RegistrationDetails.EmailAddress;

            _communicationService.SendMessageToCandidate(candidate.EntityId, MessageTypes.PasswordChanged,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, firstName),
                    new CommunicationToken(CommunicationTokens.Username, emailAddress)
                });
        }

        #endregion
    }
}
