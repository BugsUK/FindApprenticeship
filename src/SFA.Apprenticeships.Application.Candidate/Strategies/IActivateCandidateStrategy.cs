namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface IActivateCandidateStrategy
    {
        void ActivateCandidate(Guid id, string activationCode);
    }
}
