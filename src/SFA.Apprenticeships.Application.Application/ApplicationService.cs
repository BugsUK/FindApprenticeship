namespace SFA.Apprenticeships.Application.Application
{
    using System;
    using Domain.Interfaces.Repositories;
    using Interfaces.Applications;

    public class ApplicationService : IApplicationService
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;

        public ApplicationService(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
        }

        public int GetApplicationCount(string vacancyReference)
        {
            return _apprenticeshipApplicationReadRepository.GetApplicationCount(vacancyReference);
        }

        public int GetApplicationCount(int vacancyId)
        {
            return _apprenticeshipApplicationReadRepository.GetApplicationCount(vacancyId);
        }
    }
}