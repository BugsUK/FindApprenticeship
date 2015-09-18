namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    public interface IProviderUserAccountService
    {
        void SendEmailVerificationCode(string username);

        void ResendEmailVerificationCode(string username);
    }
}
