namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;

    public interface IHousekeepingStrategy
    {
        void SetSuccessor(IHousekeepingStrategy successor);

        void Handle(User user, Candidate candidate);
    }
}