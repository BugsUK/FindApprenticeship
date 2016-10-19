namespace SFA.Apprenticeships.Data.Migrate.Faa.Subscribers
{
    using Application.Application.Entities;
    using Application.Interfaces;
    using Domain.Interfaces.Messaging;

    public class TraineeshipApplicationUpdateSubscriber : IServiceBusSubscriber<TraineeshipApplicationUpdate>
    {
        private readonly ILogService _logService;

        public TraineeshipApplicationUpdateSubscriber(ILogService logService)
        {
            _logService = logService;
        }

        [ServiceBusTopicSubscription(TopicName = "TraineeshipApplicationUpdate")]
        public ServiceBusMessageStates Consume(TraineeshipApplicationUpdate request)
        {
            _logService.Debug($"Updating application with id {request.ApplicationGuid}");

            _logService.Debug($"Updated application with id {request.ApplicationGuid}");

            return ServiceBusMessageStates.Complete;
        }
    }
}