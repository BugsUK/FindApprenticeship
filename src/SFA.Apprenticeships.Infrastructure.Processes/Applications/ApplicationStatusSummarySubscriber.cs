namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System;
    using System.Threading;
    using Application.Applications;
    using Application.Applications.Entities;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;

    public class ApplicationStatusSummarySubscriber : IServiceBusSubscriber<ApplicationStatusSummary>
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;
        private readonly IServiceBus _serviceBus;
        private readonly ManualResetEvent _applicationStatusSummaryConsumerResetEvent = new ManualResetEvent(true);
        private readonly bool _strictEtlValidation;

        public ApplicationStatusSummarySubscriber(
            IApplicationStatusProcessor applicationStatusProcessor,
            IServiceBus serviceBus,
            IConfigurationService configurationService, CancellationTokenSource cancellationTokenSource)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
            _serviceBus = serviceBus;
            _strictEtlValidation = configurationService.Get<ProcessConfiguration>().StrictEtlValidation;
        }

        [ServiceBusTopicSubscription(TopicName = "application-status-summary")]
        public ServiceBusMessageResult Consume(ApplicationStatusSummary applicationStatusSummaryToProcess)
        {
            _applicationStatusProcessor.ProcessApplicationStatuses(applicationStatusSummaryToProcess, _strictEtlValidation);

            // determine whether this message is from an already propagated vacancy status summary
            var isReprocessing = applicationStatusSummaryToProcess.ApplicationId != Guid.Empty;

            if (!isReprocessing)
            {
                var vacancyStatusSummary = new VacancyStatusSummary
                {
                    LegacyVacancyId = applicationStatusSummaryToProcess.LegacyVacancyId,
                    VacancyStatus = applicationStatusSummaryToProcess.VacancyStatus,
                    ClosingDate = applicationStatusSummaryToProcess.ClosingDate
                };

                _serviceBus.PublishMessage(vacancyStatusSummary);
            }

            return ServiceBusMessageResult.Complete();
        }
    }
}
