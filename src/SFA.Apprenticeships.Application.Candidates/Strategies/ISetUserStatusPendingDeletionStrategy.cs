namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using Domain.Entities.Users;

    public interface ISetUserStatusPendingDeletionStrategy
    {
        bool SetUserStatusPendingDeletion(User user);
    }
}