namespace SFA.Apprenticeships.Data.Migrate.Faa.Subscribers
{
    using System;
    using Application.Application.Entities;
    using Application.Interfaces;
    using Domain.Interfaces.Messaging;

    public class TraineeshipApplicationUpdateSubscriber : IServiceBusSubscriber<TraineeshipApplicationUpdate>
    {
        private readonly ILogService _logService;
        private readonly ITraineeshipApplicationUpdater _traineeshipApplicationUpdater;

        public TraineeshipApplicationUpdateSubscriber(ITraineeshipApplicationUpdater traineeshipApplicationUpdater, ILogService logService)
        {
            _logService = logService;
            _traineeshipApplicationUpdater = traineeshipApplicationUpdater;
        }

        [ServiceBusTopicSubscription(TopicName = "TraineeshipApplicationUpdate")]
        public ServiceBusMessageStates Consume(TraineeshipApplicationUpdate request)
        {
            _logService.Debug($"Updating traineeship application with id {request.ApplicationGuid}");

            try
            {
                _traineeshipApplicationUpdater.Update(request.ApplicationGuid);

                _logService.Debug($"Updated traineeship application with id {request.ApplicationGuid}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to update traineeship application with id {request.ApplicationGuid}", ex);
            }

            return ServiceBusMessageStates.Complete;
        }
    }
}