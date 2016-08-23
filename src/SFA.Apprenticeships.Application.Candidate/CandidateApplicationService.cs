using SFA.Apprenticeships.Application.Interfaces.Candidates;

namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    using SFA.Apprenticeships.Application.Interfaces;
    using Strategies;
    using Strategies.Apprenticeships;
    using Strategies.Traineeships;

    public class CandidateApplicationService : ICandidateApplicationService
    {
        private readonly ILogService _logService;

        private readonly IGetCandidateByIdStrategy _getCandidateByIdStrategy;
        private readonly IGetCandidateApprenticeshipApplicationsStrategy _getCandidateApprenticeshipApplicationsStrategy;
        private readonly IGetCandidateTraineeshipApplicationsStrategy _getCandidateTraineeshipApplicationsStrategy;

        public CandidateApplicationService(ILogService logService, IGetCandidateByIdStrategy getCandidateByIdStrategy, IGetCandidateApprenticeshipApplicationsStrategy getCandidateApprenticeshipApplicationsStrategy, IGetCandidateTraineeshipApplicationsStrategy getCandidateTraineeshipApplicationsStrategy)
        {
            _logService = logService;
            _getCandidateByIdStrategy = getCandidateByIdStrategy;
            _getCandidateApprenticeshipApplicationsStrategy = getCandidateApprenticeshipApplicationsStrategy;
            _getCandidateTraineeshipApplicationsStrategy = getCandidateTraineeshipApplicationsStrategy;
        }

        public Candidate GetCandidate(Guid candidateId)
        {
            Condition.Requires(candidateId);

            _logService.Debug("Calling CandidateApplicationService to get the user with Id={0}.", candidateId);

            return _getCandidateByIdStrategy.GetCandidate(candidateId);
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