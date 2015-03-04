namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class DeleteSavedApprenticeshipApprenticeshipVacancyStrategy : IDeleteSavedApprenticeshipVacancyStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;

        public DeleteSavedApprenticeshipApprenticeshipVacancyStrategy(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        public ApprenticeshipApplicationDetail DeletedSavedVacancy(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);

            if (applicationDetail == null) return null;

            if (applicationDetail.Status != ApplicationStatuses.Saved) return applicationDetail;

            // Only actually delete a saved vacancy.
            _apprenticeshipApplicationWriteRepository.Delete(applicationDetail.EntityId);
            return null;
        }
    }
}
