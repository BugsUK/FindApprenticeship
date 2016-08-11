namespace SFA.Apprenticeships.Application.Applications.Strategies
{
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Extensions;
    using Infrastructure.Interfaces;

    public class ApplicationStatusUpdateStrategy : IApplicationStatusUpdateStrategy
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly IApplicationStatusAlertStrategy _applicationStatusAlertStrategy;

        public ApplicationStatusUpdateStrategy(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository,
            IApplicationStatusAlertStrategy applicationStatusAlertStrategy,
            ILogService logger)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _applicationStatusAlertStrategy = applicationStatusAlertStrategy;
            _logger = logger;
        }

        public void Update(
            ApprenticeshipApplicationDetail apprenticeshipApplication,
            ApplicationStatusSummary applicationStatusSummary)
        {
            var originalLegacyApplicationId = apprenticeshipApplication.LegacyApplicationId;
            var originalStatus = apprenticeshipApplication.Status;
            var originalVacancyStatus = apprenticeshipApplication.VacancyStatus;
            var originalClosingDate = apprenticeshipApplication.Vacancy.ClosingDate;
            var originalUnsuccessfulReason = apprenticeshipApplication.UnsuccessfulReason;

            // invoked because the status of the apprenticeshipApplication / vacancy *may* have changed
            if (apprenticeshipApplication.UpdateApprenticeshipApplicationDetail(applicationStatusSummary, _apprenticeshipApplicationReadRepository, _apprenticeshipApplicationWriteRepository))
            {
                const string format =
                    "Updating apprenticeship application (id='{0}', vacancy id='{1}', candidate='{2})" +
                    " from legacy application id='{3}' to '{4}'," +
                    " application status='{5}' to '{6}'," +
                    " vacancy status='{7}' to '{8}'," +
                    " closing date='{9}' to '{10}'," +
                    " unsuccessful reason='{11}' to '{12}'";

                _logger.Info(
                    format,
                    apprenticeshipApplication.EntityId, // 0
                    apprenticeshipApplication.Vacancy.Id, // 1
                    apprenticeshipApplication.CandidateDetails.EmailAddress, // 2

                    originalLegacyApplicationId, // 3
                    applicationStatusSummary.LegacyApplicationId, // 4

                    originalStatus, // 5
                    applicationStatusSummary.ApplicationStatus, // 6

                    originalVacancyStatus, // 7
                    applicationStatusSummary.VacancyStatus, // 8

                    originalClosingDate, // 9
                    applicationStatusSummary.ClosingDate, // 10

                    originalUnsuccessfulReason, // 11
                    applicationStatusSummary.UnsuccessfulReason); // 12

                _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplication);
                _applicationStatusAlertStrategy.Send(applicationStatusSummary);
            }
        }

        public void Update(TraineeshipApplicationDetail traineeshipApplication, ApplicationStatusSummary applicationStatusSummary)
        {
            var originalLegacyApplicationId = traineeshipApplication.LegacyApplicationId;
            var originalStatus = traineeshipApplication.Status;
            var originalVacancyStatus = traineeshipApplication.VacancyStatus;
            var originalClosingDate = traineeshipApplication.Vacancy.ClosingDate;

            // invoked because the status of the traineeship vacancy *may* have changed
            if (traineeshipApplication.UpdateTraineeshipApplicationDetail(applicationStatusSummary))
            {
                // note, this flow will be extended to include a call to outbound communication later (when we do notifications)
                // note, may subsequently consolidate status updates for a candidate (when we do notifications) but may be done in another component
                const string format =
                    "Updating traineeship application (id='{0}', vacancy id='{1}', candidate='{2})" +
                    " from legacy application id='{3}' to '{4}'," +
                    " application status='{5}' to '{6}'," +
                    " vacancy status='{7}' to '{8}'," +
                    " closing date='{9}' to '{10}'";

                _logger.Info(
                    format,
                    traineeshipApplication.EntityId, // 0
                    traineeshipApplication.Vacancy.Id, // 1
                    traineeshipApplication.CandidateDetails.EmailAddress, // 2

                    originalLegacyApplicationId, // 3
                    applicationStatusSummary.LegacyApplicationId, // 4

                    originalStatus, // 5
                    applicationStatusSummary.ApplicationStatus, // 6

                    originalVacancyStatus, // 7
                    applicationStatusSummary.VacancyStatus, // 8

                    originalClosingDate, // 9
                    applicationStatusSummary.ClosingDate); // 10

                _traineeshipApplicationWriteRepository.Save(traineeshipApplication);
            }
        }
    }
}