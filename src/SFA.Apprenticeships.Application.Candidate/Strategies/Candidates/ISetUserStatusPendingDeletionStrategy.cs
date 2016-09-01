namespace SFA.Apprenticeships.Application.Candidate.Strategies.Candidates
{
    using Domain.Entities.Users;

    public interface ISetUserStatusPendingDeletionStrategy
    {
        bool SetUserStatusPendingDeletion(User user);
    }
}