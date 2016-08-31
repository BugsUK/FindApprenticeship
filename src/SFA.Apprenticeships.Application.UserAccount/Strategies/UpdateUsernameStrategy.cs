namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Authentication;
    using Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    using SFA.Apprenticeships.Application.Interfaces;

    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class UpdateUsernameStrategy : IUpdateUsernameStrategy
    {
        private readonly IUserDirectoryProvider _userDirectoryProvider;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ILogService _logService;

        public UpdateUsernameStrategy(
            IUserDirectoryProvider userDirectoryProvider,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            IAuthenticationRepository authenticationRepository,
            IAuditRepository auditRepository,
            ICodeGenerator codeGenerator,
            ILogService logService)
        {
            _userDirectoryProvider = userDirectoryProvider;
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _authenticationRepository = authenticationRepository;
            _auditRepository = auditRepository;
            _codeGenerator = codeGenerator;
            _logService = logService;
        }

        public void UpdateUsername(Guid userId, string newUsername)
        {
            var user = _userReadRepository.Get(userId);
            user.PendingUsername = newUsername;
            user.PendingUsernameCode = _codeGenerator.GenerateAlphaNumeric();
            _userWriteRepository.Save(user);
        }

        public void UpdateUsername(Guid userId, string verfiyCode, string password)
        {
            var user = _userReadRepository.Get(userId);

            if (!verfiyCode.Equals(user.PendingUsernameCode, StringComparison.InvariantCultureIgnoreCase))
            {
                _logService.Debug("UpdateUsername failed to validate PendingUsernameCode: {0} for userId: {1}", verfiyCode, userId);
                throw new CustomException(ErrorCodes.InvalidUpdateUsernameCode);
            }

            if (!_userDirectoryProvider.AuthenticateUser(userId.ToString(), password))
            {
                _logService.Debug("UpdateUsername failed to autheticate userId: {0}", userId);
                throw new CustomException(ErrorCodes.UserPasswordError);
            }

            var pendingActivationUser = _userReadRepository.Get(user.PendingUsername, false);
            if (pendingActivationUser != null && pendingActivationUser.Status != UserStatuses.PendingDeletion)
            {
                //Delete any user with username = user.PendingUsername - they must be PendingActivation
                if (pendingActivationUser.Status != UserStatuses.PendingActivation)
                {
                    _logService.Error("UpdateUsername error, existing userId ({0}) to pending username ({1}) failed as username already exists and is not in PendingActivation state", userId, user.PendingUsername);
                    throw new CustomException(ErrorCodes.UsernameExistsAndNotInPendingActivationState);
                }
                _userWriteRepository.Delete(pendingActivationUser.EntityId);
                _authenticationRepository.Delete(pendingActivationUser.EntityId);
            }

            _logService.Info("UpdateUsername updating from '{0}' to '{1}'", user.Username, user.PendingUsername);
            _auditRepository.Audit(user, AuditEventTypes.UsernameChanged, user.EntityId);
            user.Username = user.PendingUsername;
            user.PendingUsername = null;
            user.PendingUsernameCode = null;
            _userWriteRepository.Save(user);
            _logService.Info("UpdateUsername updated to '{0}'", user.Username);
        }
    }
}
