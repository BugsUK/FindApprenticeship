namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public interface ICandidateApplicationService
    {
        Candidate GetCandidate(Guid candidateId);
        IList<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId, bool refresh = true);
        IList<TraineeshipApplicationSummary> GetTraineeshipApplications(Guid candidateId);
    }
}