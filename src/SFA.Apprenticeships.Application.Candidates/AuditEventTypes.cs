namespace SFA.Apprenticeships.Application.Candidates
{
    public class AuditEventTypes
    {
        public const string SetUserStatusPendingDeletion = "User.SetStatusPendingDeletion";
        public const string UsernameChanged = "User.UsernameChanged";
        public const string CandidateVerifiedMobileNumber = "Candidate.VerifiedMobileNumber";
        public const string UserResetPassword = "User.ResetPassword";
        public const string UserActivatedAccount = "User.ActivatedAccount";
        public const string HardDeleteCandidateUser = "CandidateUser.HardDelete";
    }
}