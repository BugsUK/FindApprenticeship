namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using Application.Application.Entities;
    using Application.Interfaces;
    using Configuration;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Extensions;
    using System;
    using System.Linq;

    public class ApplicationStatusChangedSubscriber : IServiceBusSubscriber<ApplicationStatusChanged>
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApplicationStatusAlertRepository _applicationStatusAlertRepository;
        private readonly ILogService _logService;
        private readonly bool _strictEtlValidation;

        public static string ApplicationNotFoundMessageFormat = "Unable to find apprenticeship application with legacy application ID '{0}'";

        public ApplicationStatusChangedSubscriber(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, IApplicationStatusAlertRepository applicationStatusAlertRepository, ILogService logService, IConfigurationService configurationService)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _applicationStatusAlertRepository = applicationStatusAlertRepository;
            _logService = logService;
            _strictEtlValidation = configurationService.Get<ProcessConfiguration>().StrictEtlValidation;
        }

        [ServiceBusTopicSubscription(TopicName = "ApprenticeshipApplicationStatusUpdated")]
        public ServiceBusMessageStates Consume(ApplicationStatusChanged applicationStatusChanged)
        {
            var application = _apprenticeshipApplicationReadRepository.Get(applicationStatusChanged.LegacyApplicationId, _strictEtlValidation);
            if (application == null)
            {
                _logService.Warn(_strictEtlValidation, string.Format(ApplicationNotFoundMessageFormat, applicationStatusChanged.LegacyApplicationId));
                return ServiceBusMessageStates.Complete;
            }

            var applicationStatusAlerts = _applicationStatusAlertRepository.GetForApplication(application.EntityId);
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
                    DateApplied = application.DateApplied ?? new DateTime(),
                    UnSuccessfulDateTime = application.UnsuccessfulDateTime ?? new DateTime()
                };
            }

            applicationStatusAlert.Status = applicationStatusChanged.ApplicationStatus;
            applicationStatusAlert.UnsuccessfulReason = applicationStatusChanged.UnsuccessfulReason;

            _applicationStatusAlertRepository.Save(applicationStatusAlert);

            return ServiceBusMessageStates.Complete;
        }
    }
}