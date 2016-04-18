namespace SFA.Apprenticeships.Application.UserAccount.Strategies.ProviderUserAccount
{
    using SFA.Apprenticeships.Domain.Entities.Communication;

    public interface ISubmitContactMessageStrategy
    {
        void SubmitMessage(ProviderContactMessage contactMessage);
    }
}
