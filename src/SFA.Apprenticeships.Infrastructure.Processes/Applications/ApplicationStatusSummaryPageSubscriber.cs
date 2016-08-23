namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using Application.Applications;
    using Application.Applications.Entities;
    using Configuration;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    public class ApplicationStatusSummaryPageSubscriber : IServiceBusSubscriber<ApplicationUpdatePage>
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;

        private readonly int _applicationStatusExtractWindow;

        public ApplicationStatusSummaryPageSubscriber(
            IConfigurationService configurationService,
            IApplicationStatusProcessor applicationStatusProcessor)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
            _applicationStatusExtractWindow = configurationService.Get<ProcessConfiguration>().ApplicationStatusExtractWindow;
        }

        [ServiceBusTopicSubscription(TopicName = "UpdateApprenticeshipApplicationStatuses")]
        public ServiceBusMessageStates Consume(ApplicationUpdatePage message)
        {
            _applicationStatusProcessor.QueueApplicationStatuses(_applicationStatusExtractWindow, message);

            return ServiceBusMessageStates.Complete;
        }
    }
}
