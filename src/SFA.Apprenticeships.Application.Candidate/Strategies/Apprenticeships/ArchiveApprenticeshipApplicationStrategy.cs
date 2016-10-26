namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class ArchiveApprenticeshipApplicationStrategy : IArchiveApplicationStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;

        public ArchiveApprenticeshipApplicationStrategy(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, IServiceBus serviceBus)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
        }

        public void ArchiveApplication(Guid candidateId, int vacancyId)
        {
            ArchiveApplication(candidateId, vacancyId, true);
        }

        public void UnarchiveApplication(Guid candidateId, int vacancyId)
        {
            ArchiveApplication(candidateId, vacancyId, false);
        }

        private void ArchiveApplication(Guid candidateId, int vacancyId, bool isArchived)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId, true);

            if (applicationDetail.IsArchived == isArchived)
            {
                // IsArchived already set to appropriate value, nothing to do.
                return;
            }

            applicationDetail.IsArchived = isArchived;
            _apprenticeshipApplicationWriteRepository.Save(applicationDetail);
        }
    }
}
