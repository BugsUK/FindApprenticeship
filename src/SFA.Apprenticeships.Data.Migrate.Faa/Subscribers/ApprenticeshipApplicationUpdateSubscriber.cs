namespace SFA.Apprenticeships.Data.Migrate.Faa.Subscribers
{
    using Application.Application.Entities;
    using Application.Interfaces;
    using Domain.Interfaces.Messaging;

    public class ApprenticeshipApplicationUpdateSubscriber : IServiceBusSubscriber<ApprenticeshipApplicationUpdate>
    {
        private readonly ILogService _logService;

        public ApprenticeshipApplicationUpdateSubscriber(ILogService logService)
        {
            _logService = logService;
        }

        [ServiceBusTopicSubscription(TopicName = "ApprenticeshipApplicationUpdate")]
        public ServiceBusMessageStates Consume(ApprenticeshipApplicationUpdate request)
        {
            _logService.Debug($"Updating application with id {request.ApplicationGuid}");

            _logService.Debug($"Updated application with id {request.ApplicationGuid}");

            return ServiceBusMessageStates.Complete;
        }
    }
}