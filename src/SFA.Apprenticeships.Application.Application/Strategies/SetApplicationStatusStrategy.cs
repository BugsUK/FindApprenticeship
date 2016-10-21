namespace SFA.Apprenticeships.Application.Application.Strategies
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using System;

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

        public void SetUnsuccessfulDecision(Guid applicationId, string candidateApplicationFeedback)
        {
            SetDecision(applicationId, ApplicationStatuses.Unsuccessful, candidateApplicationFeedback);
        }

        public void SetStateInProgress(Guid applicationId)
        {
            var application = _apprenticeshipApplicationReadRepository.Get(applicationId);
            application.SetStateInProgress();
            _apprenticeshipApplicationWriteRepository.Save(application);
            _serviceBus.PublishMessage(new ApprenticeshipApplicationUpdate(applicationId));
        }

        public void SetStateSubmitted(Guid applicationId)
        {
            var application = _apprenticeshipApplicationReadRepository.Get(applicationId);
            application.SetStateSubmitted();
            _apprenticeshipApplicationWriteRepository.Save(application);
            _serviceBus.PublishMessage(new ApprenticeshipApplicationUpdate(applicationId));
        }

        private void SetDecision(Guid applicationId, ApplicationStatuses applicationStatus, string candidateApplicationFeedback = null)
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
            if (candidateApplicationFeedback != null)
            {
                applicationStatusSummary.UnsuccessfulReason = candidateApplicationFeedback;
            }
            _applicationStatusUpdateStrategy.Update(apprenticeshipApplication, applicationStatusSummary);
            _serviceBus.PublishMessage(new ApprenticeshipApplicationUpdate(applicationId));
        }
    }
}