namespace SFA.Apprenticeships.Application.UserAccount.Strategies.ProviderUserAccount
{
    using Domain.Entities.Communication;

    public interface ISubmitContactMessageStrategy
    {
        void SubmitMessage(ProviderContactMessage contactMessage);
    }
}
