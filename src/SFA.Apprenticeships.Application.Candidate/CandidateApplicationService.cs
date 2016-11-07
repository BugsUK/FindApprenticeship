using SFA.Apprenticeships.Application.Interfaces.Candidates;

namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    using Interfaces;
    using Strategies;
    using Strategies.Apprenticeships;
    using Strategies.Candidates;
    using Strategies.Traineeships;

    public class CandidateApplicationService : ICandidateApplicationService
    {
        private readonly ILogService _logService;

        private readonly IGetCandidateByIdStrategy _getCandidateByIdStrategy;
        private readonly IGetCandidateSummariesStrategy _getCandidateSummariesStrategy;
        private readonly IGetCandidateApprenticeshipApplicationsStrategy _getCandidateApprenticeshipApplicationsStrategy;
        private readonly IGetCandidateTraineeshipApplicationsStrategy _getCandidateTraineeshipApplicationsStrategy;

        public CandidateApplicationService(ILogService logService, IGetCandidateByIdStrategy getCandidateByIdStrategy, IGetCandidateSummariesStrategy getCandidateSummariesStrategy, IGetCandidateApprenticeshipApplicationsStrategy getCandidateApprenticeshipApplicationsStrategy, IGetCandidateTraineeshipApplicationsStrategy getCandidateTraineeshipApplicationsStrategy)
        {
            _logService = logService;
            _getCandidateByIdStrategy = getCandidateByIdStrategy;
            _getCandidateSummariesStrategy = getCandidateSummariesStrategy;
            _getCandidateApprenticeshipApplicationsStrategy = getCandidateApprenticeshipApplicationsStrategy;
            _getCandidateTraineeshipApplicationsStrategy = getCandidateTraineeshipApplicationsStrategy;
        }

        public Candidate GetCandidate(int legacyCandidateId, bool errorIfNotFound = true)
        {
            Condition.Requires(legacyCandidateId);

            _logService.Debug("Calling CandidateApplicationService to get the user with legacy Id={0}.", legacyCandidateId);

            return _getCandidateByIdStrategy.GetCandidate(legacyCandidateId, errorIfNotFound);
        }

        public Candidate GetCandidate(Guid candidateId, bool errorIfNotFound = true)
        {
            Condition.Requires(candidateId);

            _logService.Debug("Calling CandidateApplicationService to get the user with Id={0}.", candidateId);

            return _getCandidateByIdStrategy.GetCandidate(candidateId, errorIfNotFound);
        }

        public IList<CandidateSummary> GetCandidateSummaries(IEnumerable<Guid> candidateIds)
        {
            return _getCandidateSummariesStrategy.GetCandidateSummaries(candidateIds);
        }

        public IList<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId, bool refresh = true)
        {
            Condition.Requires(candidateId);

            _logService.Debug(
                "Calling CandidateApplicationService to get the apprenticeship applications of the user with Id={0}.",
                candidateId);

            return _getCandidateApprenticeshipApplicationsStrategy.GetApplications(candidateId, refresh);
        }

        public IList<TraineeshipApplicationSummary> GetTraineeshipApplications(Guid candidateId)
        {
            Condition.Requires(candidateId);

            _logService.Debug(
                "Calling CandidateApplicationService to get the traineeship applications of the user with Id={0}.",
                candidateId);

            return _getCandidateTraineeshipApplicationsStrategy.GetApplications(candidateId);
        }
    }
}