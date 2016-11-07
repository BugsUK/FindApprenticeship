namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    public interface ICandidateApplicationService
    {
        Candidate GetCandidate(int legacyCandidateId, bool errorIfNotFound = true);
        Candidate GetCandidate(Guid candidateId, bool errorIfNotFound = true);
        IList<CandidateSummary> GetCandidateSummaries(IEnumerable<Guid> candidateIds);
        IList<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId, bool refresh = true);
        IList<TraineeshipApplicationSummary> GetTraineeshipApplications(Guid candidateId);
    }
}