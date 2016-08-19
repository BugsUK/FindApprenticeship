namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System;
    using Application.Applications;
    using Application.Applications.Entities;
    using Configuration;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    public class ApplicationStatusSummarySubscriber : IServiceBusSubscriber<ApplicationStatusSummary>
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;
        private readonly IServiceBus _serviceBus;
        private readonly bool _strictEtlValidation;

        public ApplicationStatusSummarySubscriber(
            IApplicationStatusProcessor applicationStatusProcessor,
            IServiceBus serviceBus,
            IConfigurationService configurationService)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
            _serviceBus = serviceBus;
            _strictEtlValidation = configurationService.Get<ProcessConfiguration>().StrictEtlValidation;
        }

        [ServiceBusTopicSubscription(TopicName = "UpdateApprenticeshipApplicationStatus")]
        public ServiceBusMessageStates Consume(ApplicationStatusSummary applicationStatusSummary)
        {
            _applicationStatusProcessor.ProcessApplicationStatuses(applicationStatusSummary, _strictEtlValidation);

            // determine whether this message is from an already propagated vacancy status summary
            var isReprocessing = applicationStatusSummary.ApplicationId != Guid.Empty;

            if (!isReprocessing)
            {
                var vacancyStatusSummary = new VacancyStatusSummary
                {
                    LegacyVacancyId = applicationStatusSummary.LegacyVacancyId,
                    VacancyStatus = applicationStatusSummary.VacancyStatus,
                    ClosingDate = applicationStatusSummary.ClosingDate
                };

                _serviceBus.PublishMessage(vacancyStatusSummary);
            }

            return ServiceBusMessageStates.Complete;
        }
    }
}
