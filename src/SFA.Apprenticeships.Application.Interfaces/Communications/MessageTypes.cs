namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    public enum MessageTypes
    {
        SendActivationCode = 0,
        SendPasswordResetCode = 1,
        SendAccountUnlockCode = 2,
        SendMobileVerificationCode = 3,
        ApprenticeshipApplicationSubmitted = 4,
        TraineeshipApplicationSubmitted = 5,
        PasswordChanged = 6,
        DailyDigest = 7,
        CandidateContactMessage = 8,
        SavedSearchAlert = 9
        //EmployerContactMessage
    }
}
