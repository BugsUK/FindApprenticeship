namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Application.Entities;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class CreateDraftApprenticeshipFromSavedVacancyStrategy : ICreateDraftApprenticeshipFromSavedVacancyStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRespository;
        private readonly IServiceBus _serviceBus;

        public CreateDraftApprenticeshipFromSavedVacancyStrategy(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRespository, IServiceBus serviceBus)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _candidateReadRespository = candidateReadRespository;
            _serviceBus = serviceBus;
        }

        public ApprenticeshipApplicationDetail CreateDraft(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);
            
            if (applicationDetail == null) return null;

            if (applicationDetail.Status != ApplicationStatuses.Saved) return null;

            var candidateDetail = _candidateReadRespository.Get(candidateId);

            if (candidateDetail == null) return null;

            // Only actually update if in saved state and we have a candidate
            applicationDetail.Status = ApplicationStatuses.Draft;
            applicationDetail.CandidateDetails = candidateDetail.RegistrationDetails;
            applicationDetail.CandidateInformation = candidateDetail.ApplicationTemplate;
            applicationDetail = _apprenticeshipApplicationWriteRepository.Save(applicationDetail);
            _serviceBus.PublishMessage(new ApprenticeshipApplicationUpdate(applicationDetail.EntityId, ApplicationUpdateType.Update));
            return applicationDetail;
        }
    }
}
