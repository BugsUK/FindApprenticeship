namespace SFA.Apprenticeships.Application.Application.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;

    public class SetApplicationStatusStrategy : ISetApplicationStatusStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly IReferenceNumberRepository _referenceNumberRepository;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;
        private readonly IServiceBus _serviceBus;

        public SetApplicationStatusStrategy(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository, IReferenceNumberRepository referenceNumberRepository, IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy, IServiceBus serviceBus)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _referenceNumberRepository = referenceNumberRepository;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
            _serviceBus = serviceBus;
        }

        public void SetSuccessfulDecision(Guid applicationId)
        {
            SetDecision(applicationId, ApplicationStatuses.Successful);
        }

        public void SetUnsuccessfulDecision(Guid applicationId)
        {
            SetDecision(applicationId, ApplicationStatuses.Unsuccessful);
        }

        public void SetStateInProgress(Guid applicationId)
        {
            var application = _apprenticeshipApplicationReadRepository.Get(applicationId);
            application.SetStateInProgress();
            _apprenticeshipApplicationWriteRepository.Save(application);
        }

        public void SetStateSubmitted(Guid applicationId)
        {
            var application = _apprenticeshipApplicationReadRepository.Get(applicationId);
            application.SetStateSubmitted();
            _apprenticeshipApplicationWriteRepository.Save(application);
        }

        private void SetDecision(Guid applicationId, ApplicationStatuses applicationStatus)
        {
            var apprenticeshipApplication = _apprenticeshipApplicationReadRepository.Get(applicationId);
            var legacyApplicationId = apprenticeshipApplication.LegacyApplicationId;
            if (legacyApplicationId == 0)
            {
                legacyApplicationId = _referenceNumberRepository.GetNextLegacyApplicationId();
            }

            var applicationStatusSummary = new ApplicationStatusSummary
            {
                // CRITICAL: make the update look like it came from legacy AVMS application
                ApplicationId = Guid.Empty,
                ApplicationStatus = applicationStatus,
                LegacyApplicationId = legacyApplicationId,
                LegacyCandidateId = 0, // not required
                LegacyVacancyId = 0, // not required
                VacancyStatus = apprenticeshipApplication.VacancyStatus,
                ClosingDate = apprenticeshipApplication.Vacancy.ClosingDate,
                UpdateSource = ApplicationStatusSummary.Source.Raa //Ensure this update is from RAA so ownership of the application is verified
            };

            _applicationStatusUpdateStrategy.Update(apprenticeshipApplication, applicationStatusSummary);
        }
    }
}