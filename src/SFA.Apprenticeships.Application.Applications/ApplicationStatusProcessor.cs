namespace SFA.Apprenticeships.Application.Applications
{
    using Application;
    using Application.Entities;
    using Application.Strategies;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Interfaces;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicationStatusProcessor : IApplicationStatusProcessor
    {
        private readonly ILogService _logger;
        private readonly IServiceBus _serviceBus;

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IApplicationStatusUpdateStrategy _applicationStatusUpdateStrategy;

        public ApplicationStatusProcessor(
            ILogService logger,
            IServiceBus serviceBus,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ICandidateReadRepository candidateReadRepository,
            IApplicationStatusUpdateStrategy applicationStatusUpdateStrategy)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _applicationStatusUpdateStrategy = applicationStatusUpdateStrategy;
        }

        public void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary, bool strictEtlValidation)
        {
            _logger.Debug("Processing application summary status update for application with legacy application ID '{0}'", applicationStatusSummary.LegacyApplicationId);

            if (!ProcessApprenticeshipApplication(applicationStatusSummary, strictEtlValidation) && !ProcessTraineeshipApplication(applicationStatusSummary))
            {
                var message = string.Format("Unable to find/update apprenticeship or traineeship application status for application with legacy application ID '{0}', application ID '{1}' and legacy candidate ID '{2}'. Check the audit repository to see if the candidate and application was deleted", applicationStatusSummary.LegacyApplicationId, applicationStatusSummary.ApplicationId, applicationStatusSummary.LegacyCandidateId);

                if (applicationStatusSummary.ApplicationId != Guid.Empty && applicationStatusSummary.LegacyApplicationId == 0)
                {
                    _logger.Info(message + ". It was likely a draft that was deleted by the candidate.");
                }
                else
                {
                    if (strictEtlValidation)
                    {
                        _logger.Warn(message);
                    }
                    else
                    {
                        _logger.Info(message);
                    }
                }
            }
        }

        public void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary)
        {
            // propagate current vacancy state to all draft applications for the vacancy
            var applicationSummaries = _apprenticeshipApplicationReadRepository.GetApplicationSummaries(vacancyStatusSummary.LegacyVacancyId);

            var applicationStatusSummaries = applicationSummaries
                .Where(applicationSummary => applicationSummary.Status == ApplicationStatuses.Draft)
                .Select(applicationSummary =>
                    new ApplicationStatusSummary
                    {
                        ApplicationId = applicationSummary.ApplicationId,
                        ApplicationStatus = applicationSummary.Status,
                        LegacyApplicationId = applicationSummary.LegacyApplicationId,
                        LegacyVacancyId = applicationSummary.LegacyVacancyId,
                        VacancyStatus = vacancyStatusSummary.VacancyStatus,
                        ClosingDate = vacancyStatusSummary.ClosingDate,
                        UnsuccessfulReason = applicationSummary.UnsuccessfulReason
                    });

            Parallel.ForEach(
                applicationStatusSummaries,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                applicationStatusSummary => _serviceBus.PublishMessage(applicationStatusSummary));
        }

        private bool ProcessApprenticeshipApplication(ApplicationStatusSummary applicationStatusSummary, bool strictEtlValidation)
        {
            var apprenticeshipApplicationDetail = default(ApprenticeshipApplicationDetail);

            if (applicationStatusSummary.ApplicationId != Guid.Empty)
            {
                apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.ApplicationId, false);
            }

            if (apprenticeshipApplicationDetail == null && applicationStatusSummary.LegacyApplicationId != 0)
            {
                apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId, strictEtlValidation);
            }

            if (apprenticeshipApplicationDetail == null && applicationStatusSummary.LegacyCandidateId != 0)
            {
                // in some cases the application can't be found using the application IDs so use legacy candidate and vacancy IDs
                var candidate = _candidateReadRepository.Get(applicationStatusSummary.LegacyCandidateId, false);

                if (candidate == null)
                {
                    return false;
                }

                apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, applicationStatusSummary.LegacyVacancyId);
            }

            if (apprenticeshipApplicationDetail == null)
            {
                return false; // not necessarily an error as may be a traineeship
            }

            _applicationStatusUpdateStrategy.Update(apprenticeshipApplicationDetail, applicationStatusSummary);

            return true;
        }

        private bool ProcessTraineeshipApplication(ApplicationStatusSummary applicationStatusSummary)
        {
            var traineeshipApplicationDetail = _traineeshipApplicationReadRepository.Get(applicationStatusSummary.LegacyApplicationId);

            if (traineeshipApplicationDetail == null && applicationStatusSummary.LegacyCandidateId != 0)
            {
                // in some cases the application can't be found using the application IDs so use legacy candidate and vacancy IDs
                var candidate = _candidateReadRepository.Get(applicationStatusSummary.LegacyCandidateId, false);

                if (candidate == null)
                {
                    return false;
                }

                traineeshipApplicationDetail = _traineeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, applicationStatusSummary.LegacyVacancyId);
            }

            if (traineeshipApplicationDetail == null)
            {
                return false;
            }

            _applicationStatusUpdateStrategy.Update(traineeshipApplicationDetail, applicationStatusSummary);

            return true;
        }
    }
}
