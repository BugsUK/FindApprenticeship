namespace SFA.Apprenticeships.Application.UserAccount.Strategies.ProviderUserAccount
{
    public interface IResendEmailVerificationCodeStrategy
    {
        void ResendEmailVerificationCode(string username);
    }
}
