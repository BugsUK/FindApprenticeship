namespace SFA.Apprenticeships.Application.Interfaces
{
    public class AuditEventTypes
    {
        public const string UsernameChanged = "User.UsernameChanged";
        public const string CandidateVerifiedMobileNumber = "Candidate.VerifiedMobileNumber";
        public const string UserResetPassword = "User.ResetPassword";
        public const string UserRegisterAccount = "User.RegisterAccount";
        public const string UserActivatedAccount = "User.ActivatedAccount";
        public const string CandidateUserMakeDormant = "CandidateUser.MakeDormant";
        public const string UserSoftDelete = "User.SoftDelete";
        public const string HardDeleteCandidateUser = "CandidateUser.HardDelete";
        public const string HardDeleteApprenticeshipApplication = "ApprenticeshipApplication.HardDelete";
        public const string HardDeleteTraineeshipApplication = "TraineeshipApplication.HardDelete";
    }
}