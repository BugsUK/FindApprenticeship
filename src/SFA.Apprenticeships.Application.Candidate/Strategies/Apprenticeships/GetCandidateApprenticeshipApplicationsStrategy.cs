namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class GetCandidateApprenticeshipApplicationsStrategy : IGetCandidateApprenticeshipApplicationsStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;

        public GetCandidateApprenticeshipApplicationsStrategy(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
        }

        public IList<ApprenticeshipApplicationSummary> GetApplications(Guid candidateId, bool refresh = true)
        {
            return _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId);
        }
    }
}
