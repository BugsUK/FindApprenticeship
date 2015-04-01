namespace SFA.Apprenticeships.Application.Candidate
{
    using Domain.Entities.Candidates;

    public interface ILegacyCandidateProvider
    {
        int CreateCandidate(Candidate candidate);

        void UpdateCandidate(Candidate candidate);
    }
}
