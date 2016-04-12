namespace SFA.Apprenticeships.Application.Provider.Strategies
{
    using SFA.Apprenticeships.Domain.Entities.Communication;

    public interface ISubmitContactMessageStrategy
    {
        void SubmitMessage(ContactMessage contactMessage);
    }
}
