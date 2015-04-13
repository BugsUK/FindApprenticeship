namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;

    public interface ICandidateMetricsRepository
    {
        int GetVerfiedMobileNumbersCount();

        int GetDismissedTraineeshipPromptCount();

        IEnumerable<Guid> GetCandidatesThatHaveDismissedTheTraineeshipPrompt();
    }
}