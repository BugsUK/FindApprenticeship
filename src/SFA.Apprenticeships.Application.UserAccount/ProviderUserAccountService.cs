namespace SFA.Apprenticeships.Application.UserAccount
{
    using CuttingEdge.Conditions;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Users;
    using SFA.Apprenticeships.Domain.Entities.Communication;
    using Strategies.ProviderUserAccount;

    public class ProviderUserAccountService : IProviderUserAccountService
    {
        private readonly ILogService _logService;
        private readonly ISendEmailVerificationCodeStrategy _sendEmailVerificationCodeStrategy;
        private readonly IResendEmailVerificationCodeStrategy _resendEmailVerificationCodeStrategy;
        private readonly ISubmitContactMessageStrategy _submitContactMessageStrategy;

        public ProviderUserAccountService(
            ILogService logService,
            ISendEmailVerificationCodeStrategy sendEmailVerificationCodeStrategy,
            IResendEmailVerificationCodeStrategy resendEmailVerificationCodeStrategy,
            ISubmitContactMessageStrategy submitContactMessageStrategy)
        {
            _sendEmailVerificationCodeStrategy = sendEmailVerificationCodeStrategy;
            _resendEmailVerificationCodeStrategy = resendEmailVerificationCodeStrategy;
            _logService = logService;
            _submitContactMessageStrategy = submitContactMessageStrategy;

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

        public void SubmitContactMessage(ProviderContactMessage contactMessage)
        {
            Condition.Requires(contactMessage);
            _submitContactMessageStrategy.SubmitMessage(contactMessage);
        }
    }
}
