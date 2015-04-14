namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Authentication;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;
    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class UpdateUsernameStrategy : IUpdateUsernameStrategy
    {
        private readonly IUserDirectoryProvider _userDirectoryProvider;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ICodeGenerator _codeGenerator;

        public UpdateUsernameStrategy(
            IUserDirectoryProvider userDirectoryProvider,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
            ICodeGenerator codeGenerator)
        {
            _userDirectoryProvider = userDirectoryProvider;
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _codeGenerator = codeGenerator;
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
            if (!_userDirectoryProvider.AuthenticateUser(userId.ToString(), password))
            {
                throw new CustomException(ErrorCodes.UserPasswordError);
            }

            var user = _userReadRepository.Get(userId);

            if (!verfiyCode.Equals(user.PendingUsernameCode, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new CustomException(ErrorCodes.InvalidUpdateUsernameCode);
            }

            //TODO: delete any users with username = user.PendingUsername - they must be PendingActivation
            //TODO: delete user authentication data for them as well.
            //var pendingActivationUser = _userReadRepository.Get(user.PendingUsername);
            //if (pendingActivationUser != null)
            //{
            //    if (pendingActivationUser.Status != UserStatuses.PendingActivation)
            //    {
            //        throw new CustomException(ErrorCodes.TBC);
            //    }
            //    _userWriteRepository.Delete(pendingActivationUser.EntityId);
            //}

            user.Username = user.PendingUsername;
            user.PendingUsername = null;
            user.PendingUsernameCode = null;
            _userWriteRepository.Save(user);
        }
    }
}
