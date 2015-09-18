namespace SFA.Apprenticeships.Application.UserAccount.Strategies.ProviderUserAccount
{
    public interface ISendEmailVerificationCodeStrategy
    {
        void SendEmailVerificationCode(string username);
    }
}
