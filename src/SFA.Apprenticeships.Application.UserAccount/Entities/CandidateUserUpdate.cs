namespace SFA.Apprenticeships.Application.UserAccount.Entities
{
    using System;

    public class CandidateUserUpdate
    {
        public CandidateUserUpdate(Guid candidateGuid, CandidateUserUpdateType candidateUserUpdateType)
        {
            CandidateGuid = candidateGuid;
            CandidateUserUpdateType = candidateUserUpdateType;
        }

        public Guid CandidateGuid { get; private set; }

        public CandidateUserUpdateType CandidateUserUpdateType { get; private set; }
    }
}