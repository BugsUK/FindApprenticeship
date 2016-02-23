namespace SFA.Apprenticeships.Application.UserAccount.Strategies.ProviderUserAccount
{
    using Domain.Entities.Exceptions;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Communications;
    using Interfaces.Users;
    using ErrorCodes = Interfaces.Users.ErrorCodes;

    public class SendEmailVerificationCodeStrategy : ISendEmailVerificationCodeStrategy
    {
        public const int EmailVerificationCodeLength = 6;

        private readonly IProviderUserReadRepository _providerUserReadRepository;
        private readonly IProviderUserWriteRepository _providerUserWriteRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IProviderCommunicationService _communicationService;

        public SendEmailVerificationCodeStrategy(
            IProviderUserReadRepository providerUserReadRepository,
            IProviderUserWriteRepository providerUserWriteRepository,
            ICodeGenerator codeGenerator,
            IProviderCommunicationService communicationService)
        {
            _providerUserReadRepository = providerUserReadRepository;
            _providerUserWriteRepository = providerUserWriteRepository;
            _codeGenerator = codeGenerator;
            _communicationService = communicationService;
        }

        public void SendEmailVerificationCode(string username)
        {
            var providerUser = _providerUserReadRepository.Get(username);

            if (providerUser == null)
            {
                throw new CustomException("Unknown username", ErrorCodes.UnknownUserError);
            }

            // ReSharper disable once RedundantArgumentDefaultValue
            providerUser.EmailVerificationCode = _codeGenerator.GenerateAlphaNumeric(EmailVerificationCodeLength);

            _providerUserWriteRepository.Save(providerUser);

            _communicationService.SendMessageToProviderUser(username, MessageTypes.SendProviderUserEmailVerificationCode,
                new[]
                {
                    new CommunicationToken(CommunicationTokens.ProviderUserUsername, providerUser.Username),
                    new CommunicationToken(CommunicationTokens.ProviderUserEmailVerificationCode, providerUser.EmailVerificationCode)
                });
        }
    }
}
