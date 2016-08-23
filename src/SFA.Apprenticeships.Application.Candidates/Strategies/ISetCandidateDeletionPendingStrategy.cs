namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using Domain.Entities.Users;

    public interface ISetCandidateDeletionPendingStrategy
    {
        bool SetUserStatusPendingDeletion(User user);
    }
}