namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;

    public interface IGetCandidateByUsernameStrategy
    {
        Candidate GetCandidate(string username);
    }
}