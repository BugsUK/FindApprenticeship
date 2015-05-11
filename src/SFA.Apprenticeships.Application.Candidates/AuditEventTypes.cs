namespace SFA.Apprenticeships.Application.Candidates
{
    public class AuditEventTypes
    {
        public const string SetCandidateStatusPendingDeletion = "Candidate.SetStatusPendingDeletion";
        public const string UsernameChanged = "User.UsernameChanged";
        public const string CandidateVerifiedMobileNumber = "Candidate.VerifiedMobileNumber";
        public const string UserResetPassword = "User.ResetPassword";
        public const string UserActivatedAccount = "User.ActivatedAccount";
        public const string HardDeleteUser = "User.HardDeleteUser";
        public const string HardDeleteCandidate = "User.HardDeleteCandidate";
    }
}