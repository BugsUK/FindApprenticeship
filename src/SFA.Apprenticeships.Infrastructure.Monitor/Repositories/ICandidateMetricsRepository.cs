namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;

    public interface ICandidateMetricsRepository
    {
        int GetVerfiedMobileNumbersCount();

        int GetDismissedTraineeshipPromptCount();
    }
}