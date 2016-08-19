namespace SFA.Apprenticeships.Application.UserAccount.Strategies.ProviderUserAccount
{
    using Interfaces.Communications;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Users;
    using Domain.Raa.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;
    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class ResendEmailVerificationCodeStrategy : IResendEmailVerificationCodeStrategy
    {
        private readonly ILogService _logService;
        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IProviderCommunicationService _communicationService;

        public ResendEmailVerificationCodeStrategy(
            ILogService logService,
            IProviderUserReadRepository providerUserReadRepository,
            IProviderCommunicationService communicationService)
        {
            _logService = logService;
            _providerUserReadRepository = providerUserReadRepository;
            _communicationService = communicationService;
        }

        public void ResendEmailVerificationCode(string username)
        {
            var providerUser = _providerUserReadRepository.GetByUsername(username);

            if (providerUser == null)
            {
                throw new CustomException("Unknown username", ErrorCodes.UnknownUserError);
            }

            if (providerUser.Status == ProviderUserStatus.EmailVerified)
            {
                _logService.Info("Will not resend provider user email verification code, verification code is blank.");
                return;
            }

            _communicationService.SendMessageToProviderUser(username, MessageTypes.SendProviderUserEmailVerificationCode,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.ProviderUserUsername, providerUser.Username),
                    new CommunicationToken(CommunicationTokens.ProviderUserEmailVerificationCode, providerUser.EmailVerificationCode)
                });
        }
    }
}
