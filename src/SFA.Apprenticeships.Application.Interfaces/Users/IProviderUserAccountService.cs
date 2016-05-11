namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using SFA.Apprenticeships.Domain.Entities.Communication;

    public interface IProviderUserAccountService
    {
        void SendEmailVerificationCode(string username);

        void ResendEmailVerificationCode(string username);

        void SubmitContactMessage(ProviderContactMessage contactMessage);
    }
}
