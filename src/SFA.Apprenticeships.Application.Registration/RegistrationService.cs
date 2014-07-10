﻿namespace SFA.Apprenticeships.Application.Registration
{
    using System;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    public class RegistrationService : IRegistrationService
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;

        public RegistrationService(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        public bool IsUsernameAvailable(string username)
        {
            return _userReadRepository.Get(username, false) == null;
        }

        public void Register(string username, Guid userId, string activationCode, UserRoles roles)
        {
            var user = new User
            {
                ActivationCode = activationCode,
                Status = UserStatuses.PendingActivation,
                EntityId = userId,
                Username = username,
                PasswordResetCode = string.Empty,
                Roles = roles
            };

            _userWriteRepository.Save(user);
        }

        public void Activate(string username, string activationCode)
        {
            var user = _userReadRepository.Get(username);

            if (!user.ActivationCode.Equals(activationCode, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("Invalid activation code"); //todo: should use an application exception type

            user.Status = UserStatuses.Active;
            user.ActivationCode = string.Empty;

            _userWriteRepository.Save(user);
        }

        public void ResendActivationCode(string username)
        {
            throw new NotImplementedException();
        }

        public void ResendPasswordCode(string username)
        {
            throw new NotImplementedException();
        }

        public void ChangeForgottenPassword(string username, string passwordCode, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
