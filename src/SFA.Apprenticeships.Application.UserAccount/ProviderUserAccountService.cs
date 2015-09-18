namespace SFA.Apprenticeships.Application.UserAccount
{
    using CuttingEdge.Conditions;
    using Interfaces.Logging;
    using Interfaces.Users;
    using Strategies.ProviderUserAccount;

    public class ProviderUserAccountService : IProviderUserAccountService
    {
        private readonly ILogService _logService;

        private readonly ISendEmailVerificationCodeStrategy _sendEmailVerificationCodeStrategy;
        private readonly IResendEmailVerificationCodeStrategy _resendEmailVerificationCodeStrategy;

        public ProviderUserAccountService(
            ILogService logService,
            ISendEmailVerificationCodeStrategy sendEmailVerificationCodeStrategy,
            IResendEmailVerificationCodeStrategy resendEmailVerificationCodeStrategy)
        {
            _sendEmailVerificationCodeStrategy = sendEmailVerificationCodeStrategy;
            _resendEmailVerificationCodeStrategy = resendEmailVerificationCodeStrategy;
            _logService = logService;
        }

        public void SendEmailVerificationCode(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logService.Debug("Calling UserAccountService to send the account unlock code for the provider user {0}.", username);

            _sendEmailVerificationCodeStrategy.SendEmailVerificationCode(username);
        }

        public void ResendEmailVerificationCode(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            _logService.Debug("Calling UserAccountService to resend the account unlock code for the provider user {0}.", username);

            _resendEmailVerificationCodeStrategy.ResendEmailVerificationCode(username);
        }
    }
}
