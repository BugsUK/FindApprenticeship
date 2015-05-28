namespace SFA.Apprenticeships.Application.Candidates
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;

    public interface IHousekeepingChainOfResponsibility
    {
        void Handle(User user, Candidate candidate); 
    }
}