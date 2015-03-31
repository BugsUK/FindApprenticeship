namespace SFA.Apprenticeships.Application.Candidate
{
    using Domain.Entities.Candidates;

    public interface ILegacyCandidateProvider
    {
        int CreateCandidate(Candidate candidate);

        //todo: 1.9: void UpdateCandidate(Candidate candidate);
    }
}
