namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Application.Entities;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class DeleteSavedApprenticeshipApprenticeshipVacancyStrategy : IDeleteSavedApprenticeshipVacancyStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly IServiceBus _serviceBus;

        public DeleteSavedApprenticeshipApprenticeshipVacancyStrategy(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, IServiceBus serviceBus)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _serviceBus = serviceBus;
        }

        public ApprenticeshipApplicationDetail DeleteSavedVacancy(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);

            if (applicationDetail == null) return null;

            if (applicationDetail.Status != ApplicationStatuses.Saved) return applicationDetail;

            // Only actually delete a saved vacancy.
            _apprenticeshipApplicationWriteRepository.Delete(applicationDetail.EntityId);
            _serviceBus.PublishMessage(new ApprenticeshipApplicationUpdate(applicationDetail.EntityId, ApplicationUpdateType.Delete));
            return null;
        }
    }
}
