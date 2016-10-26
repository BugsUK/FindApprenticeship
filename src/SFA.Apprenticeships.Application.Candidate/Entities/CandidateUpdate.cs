namespace SFA.Apprenticeships.Application.Candidate.Entities
{
    using System;

    public class CandidateUpdate
    {
        protected CandidateUpdate(Guid candidateGuid, CandidateUpdateType candidateUpdateType)
        {
            CandidateGuid = candidateGuid;
            CandidateUpdateType = candidateUpdateType;
        }

        public Guid CandidateGuid { get; private set; }

        public CandidateUpdateType CandidateUpdateType { get; private set; }
    }
}