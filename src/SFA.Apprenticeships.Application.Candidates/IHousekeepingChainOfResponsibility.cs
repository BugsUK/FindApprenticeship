namespace SFA.Apprenticeships.Application.Candidates
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;

    public interface IHousekeepingChainOfResponsibility
    {
        int Order { get; }

        void Handle(User user, Candidate candidate); 
    }
}