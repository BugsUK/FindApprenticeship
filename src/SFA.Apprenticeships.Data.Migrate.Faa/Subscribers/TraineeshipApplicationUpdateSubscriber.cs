namespace SFA.Apprenticeships.Data.Migrate.Faa.Subscribers
{
    using System;
    using System.Data.SqlClient;
    using Application.Application.Entities;
    using Application.Interfaces;
    using Domain.Entities.Exceptions;
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
            _logService.Debug($"Processing traineeship application update with id {request.ApplicationGuid} and type {request.ApplicationUpdateType}");

            try
            {
                switch (request.ApplicationUpdateType)
                {
                    case ApplicationUpdateType.Create:
                        _traineeshipApplicationUpdater.Create(request.ApplicationGuid);
                        _logService.Debug($"Created traineeship application with id {request.ApplicationGuid}");
                        break;
                    case ApplicationUpdateType.Update:
                        _traineeshipApplicationUpdater.Update(request.ApplicationGuid);
                        _logService.Debug($"Updated traineeship application with id {request.ApplicationGuid}");
                        break;
                    case ApplicationUpdateType.Delete:
                        _traineeshipApplicationUpdater.Delete(request.ApplicationGuid);
                        _logService.Debug($"Deleted traineeship application with id {request.ApplicationGuid}");
                        break;
                    default:
                        _logService.Warn($"Traineeship application update with id {request.ApplicationGuid} was of an unknown or unsupported type {request.ApplicationUpdateType}. Dead lettering message");
                        return ServiceBusMessageStates.DeadLetter;
                }

                return ServiceBusMessageStates.Complete;
            }
            catch (CustomException ex)
            {
                _logService.Error($"Failed to process traineeship application update with id {request.ApplicationGuid} and type {request.ApplicationUpdateType}. Requeuing message", ex);
                return ServiceBusMessageStates.Requeue;
            }
            catch (SqlException ex)
            {
                _logService.Warn($"Failed to process traineeship application update with id {request.ApplicationGuid} and type {request.ApplicationUpdateType}. Requeuing message", ex);
                return ServiceBusMessageStates.Requeue;
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to process traineeship application update with id {request.ApplicationGuid} and type {request.ApplicationUpdateType}. Dead lettering message", ex);
                return ServiceBusMessageStates.DeadLetter;
            }
        }
    }
}