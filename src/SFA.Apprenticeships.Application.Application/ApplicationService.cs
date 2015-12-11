namespace SFA.Apprenticeships.Application.Application
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;

    public class ApplicationService : IApplicationService
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;

        public ApplicationService(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
        }

        public IEnumerable<ApprenticeshipApplicationSummary> GetSubmittedApplicationSummaries(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetSubmittedApplicationSummaries(vacancyId);
        }

        public int GetApplicationCount(string vacancyReference)
        {
            return _apprenticeshipApplicationReadRepository.GetApplicationCount(vacancyReference);
        }

        public int GetApplicationCount(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetApplicationCount(vacancyId);
        }

        public ApprenticeshipApplicationDetail GetApplication(Guid applicationId)
        {
            return _apprenticeshipApplicationReadRepository.Get(applicationId);
        }
    }
}