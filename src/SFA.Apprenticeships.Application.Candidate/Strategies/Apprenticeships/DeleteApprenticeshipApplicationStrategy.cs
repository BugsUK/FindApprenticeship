namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Application.Entities;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class DeleteApprenticeshipApplicationStrategy : IDeleteApplicationStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly IServiceBus _serviceBus;

        public DeleteApprenticeshipApplicationStrategy(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, IServiceBus serviceBus)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _serviceBus = serviceBus;
        }

        public void DeleteApplication(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);

            if (applicationDetail != null)
            {
                applicationDetail.AssertState("Delete application", ApplicationStatuses.Saved, ApplicationStatuses.Draft, ApplicationStatuses.ExpiredOrWithdrawn);

                _apprenticeshipApplicationWriteRepository.Delete(applicationDetail.EntityId);
                _serviceBus.PublishMessage(new ApprenticeshipApplicationUpdate(applicationDetail.EntityId, ApplicationUpdateType.Delete));
            }
        }
    }
}