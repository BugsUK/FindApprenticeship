namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;

    public class RegisterUserStrategy : IRegisterUserStrategy
    {
        private readonly int _activationCodeExpiryDays;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;

        public RegisterUserStrategy(IUserWriteRepository userWriteRepository, 
            IConfigurationService configurationService,
            IUserReadRepository userReadRepository)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _activationCodeExpiryDays = configurationService.Get<UserAccountConfiguration>(UserAccountConfiguration.ConfigurationName).ActivationCodeExpiryDays;
        }

        public void Register(string username, Guid userId, string activationCode, UserRoles roles)
        {
            var user = _userReadRepository.Get(username, false);

            if (user != null && user.Status != UserStatuses.PendingActivation)
            {
                throw new CustomException("Username already in use and is not in pending activation status", Domain.Entities.ErrorCodes.EntityStateError);
            }

            var newUser = new User
            {
                EntityId = userId,
                Username = username,
                Roles = roles
            };

            newUser.SetStatePendingActivation(activationCode, DateTime.Now.AddDays(_activationCodeExpiryDays));
            _userWriteRepository.Save(newUser);
        }
    }
}