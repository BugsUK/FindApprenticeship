namespace SFA.Apprenticeships.Application.Candidate.Strategies.Candidates
{
    using Domain.Entities.Users;

    public interface ISetCandidateDeletionPendingStrategy
    {
        bool SetUserStatusPendingDeletion(User user);
    }
}