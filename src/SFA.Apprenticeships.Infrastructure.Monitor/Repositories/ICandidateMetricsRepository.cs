namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;

    public interface ICandidateMetricsRepository
    {
        int GetVerfiedMobileNumbersCount();

        int GetDismissedTraineeshipPromptCount();

        IEnumerable<Guid> GetCandidatesThatHaveDismissedTheTraineeshipPrompt();

        IEnumerable<Candidate> GetCandidateActivityMetrics(DateTime dateCreatedStart, DateTime dateCreatedEnd);
    }
}