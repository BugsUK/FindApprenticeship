namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Applications.Entities;
    using Application.Interfaces.Logging;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;

    public class ApplicationStatusChangedConsumerAsync : IConsumeAsync<ApplicationStatusChanged>
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApplicationStatusAlertRepository _applicationStatusAlertRepository;
        private readonly ILogService _logService;

        public static string ApplicationNotFoundMessageFormat = "Unable to find apprenticeship application with legacy application ID '{0}'";

        public ApplicationStatusChangedConsumerAsync(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, IApplicationStatusAlertRepository applicationStatusAlertRepository, ILogService logService)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            _logService = logService;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "ApplicationStatusChangedConsumerAsync")]
        public Task Consume(ApplicationStatusChanged applicationStatusChanged)
        {
            return Task.Run(() =>
            {
                var application = _apprenticeshipApplicationReadRepository.Get(applicationStatusChanged.LegacyApplicationId);
                if (application == null)
                {
                    _logService.Warn(string.Format(ApplicationNotFoundMessageFormat, applicationStatusChanged.LegacyApplicationId));
                    return;
                }

                var applicationStatusAlerts = _applicationStatusAlertRepository.Get(application.EntityId);
                var applicationStatusAlert = applicationStatusAlerts.FirstOrDefault(asa => asa.BatchId == null);
                if (applicationStatusAlert == null)
                {
                    applicationStatusAlert = new ApplicationStatusAlert
                    {
                        CandidateId = application.CandidateId,
                        ApplicationId = application.EntityId,
                        VacancyId = application.Vacancy.Id,
                        Title = application.Vacancy.Title,
                        EmployerName = application.Vacancy.EmployerName,
                        Status = application.Status,
                        UnsuccessfulReason = application.UnsuccessfulReason,
                        DateApplied = application.DateApplied ?? new DateTime()
                    };
                }
                applicationStatusAlert.Status = applicationStatusChanged.ApplicationStatus;
                applicationStatusAlert.UnsuccessfulReason = applicationStatusChanged.UnsuccessfulReason;

                _applicationStatusAlertRepository.Save(applicationStatusAlert);
            });
        }
    }
}