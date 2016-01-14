namespace SFA.Apprenticeships.Infrastructure.UserDirectory
{
    using System;
    using Application.Authentication;
    using SFA.Infrastructure.Interfaces;
    using Application.UserAccount.Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Hash;
    using UsersErrorCodes = Application.Interfaces.Users.ErrorCodes;

    public class UserDirectoryProvider : IUserDirectoryProvider
    {
        private readonly ILogService _logger;

        private readonly IConfigurationService _configurationService;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IPasswordHash _passwordHash;

        public UserDirectoryProvider(
            IConfigurationService configurationService,
            IAuthenticationRepository authenticationRepository,
            IPasswordHash passwordHash, ILogService logger)
        {
            _configurationService = configurationService;
            _authenticationRepository = authenticationRepository;
            _passwordHash = passwordHash;
            _logger = logger;
        }

        public bool AuthenticateUser(string userId, string password)
        {
            _logger.Debug("Authenticating userId={0}", userId);

            var userCredentials = _authenticationRepository.Get(new Guid(userId), true);

            var isValidated = _passwordHash.Validate(userCredentials.PasswordHash, userId, password, SecretKey);

            var message = isValidated
                        ? "Successfully validated credentials for Id={0}"
                        : "Failed to validate credentials for Id={0}";

            _logger.Debug(message, userId);

            return isValidated;
        }

        public bool CreateUser(string userId, string password)
        {
            _logger.Debug("Creating user authentication for userId={0}", userId);

            var userCredentials = _authenticationRepository.Get(new Guid(userId));
            if (userCredentials != null)
            {
                var message = string.Format("User authentication exists for userId={0}", userId);
                _logger.Debug(message);
                throw new CustomException(message, UsersErrorCodes.UserDirectoryAccountExistsError);
            }

            _logger.Debug("No user authentication found for userId={0}", userId);

            var hash = _passwordHash.Generate(userId, password, SecretKey);

            _authenticationRepository.Save(new UserCredentials
            {
                EntityId = new Guid(userId),
                PasswordHash = hash
            });

            _logger.Debug("Created user authentication for userId={0}", userId);

            return true;
        }

        public bool ResetPassword(string userId, string newpassword)
        {
            _logger.Debug("Resetting password for userId={0}", userId);

            return SetUserPassword(userId, newpassword);
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            _logger.Debug("Changing password for userId={0}", userId);

            return AuthenticateUser(userId, oldPassword) && SetUserPassword(userId, newPassword);
        }

        private bool SetUserPassword(string userId, string newPassword)
        {
            var userCredentials = _authenticationRepository.Get(new Guid(userId), true);
 
            var hash = _passwordHash.Generate(userId, newPassword, SecretKey);
            userCredentials.PasswordHash = hash;

            _logger.Debug("Saving new password for userId={0}", userId);
            _authenticationRepository.Save(userCredentials);

            return true;
        }

        private string SecretKey
        {
            get
            {
                var config = _configurationService.Get<UserAccountConfiguration>();
                return config.UserDirectorySecretKey;
            }
        }
    }
}