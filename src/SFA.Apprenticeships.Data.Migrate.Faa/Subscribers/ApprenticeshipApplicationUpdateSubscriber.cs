namespace SFA.Apprenticeships.Data.Migrate.Faa.Subscribers
{
    using System;
    using Application.Application.Entities;
    using Application.Interfaces;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;

    public class ApprenticeshipApplicationUpdateSubscriber : IServiceBusSubscriber<ApprenticeshipApplicationUpdate>
    {
        private readonly ILogService _logService;
        private readonly IApprenticeshipApplicationUpdater _apprenticeshipApplicationUpdater;

        public ApprenticeshipApplicationUpdateSubscriber(IApprenticeshipApplicationUpdater apprenticeshipApplicationUpdater, ILogService logService)
        {
            _logService = logService;
            _apprenticeshipApplicationUpdater = apprenticeshipApplicationUpdater;
        }

        [ServiceBusTopicSubscription(TopicName = "ApprenticeshipApplicationUpdate")]
        public ServiceBusMessageStates Consume(ApprenticeshipApplicationUpdate request)
        {
            _logService.Debug($"Processing apprenticeship application update with id {request.ApplicationGuid} and type {request.ApplicationUpdateType}");

            try
            {
                switch (request.ApplicationUpdateType)
                {
                    case ApplicationUpdateType.Create:
                        _apprenticeshipApplicationUpdater.Create(request.ApplicationGuid);
                        _logService.Debug($"Created apprenticeship application with id {request.ApplicationGuid}");
                        break;
                    case ApplicationUpdateType.Update:
                        _apprenticeshipApplicationUpdater.Update(request.ApplicationGuid);
                        _logService.Debug($"Updated apprenticeship application with id {request.ApplicationGuid}");
                        break;
                    case ApplicationUpdateType.Delete:
                        _apprenticeshipApplicationUpdater.Delete(request.ApplicationGuid);
                        _logService.Debug($"Deleted apprenticeship application with id {request.ApplicationGuid}");
                        break;
                    default:
                        _logService.Warn($"Apprenticeship application update with id {request.ApplicationGuid} was of an unknown or unsupported type {request.ApplicationUpdateType}. Dead lettering message");
                        return ServiceBusMessageStates.DeadLetter;
                }

                return ServiceBusMessageStates.Complete;
            }
            catch (CustomException ex)
            {
                _logService.Error($"Failed to process apprenticeship application update with id {request.ApplicationGuid} and type {request.ApplicationUpdateType}. Requeuing message", ex);
                return ServiceBusMessageStates.Requeue;
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to process apprenticeship application update with id {request.ApplicationGuid} and type {request.ApplicationUpdateType}. Dead lettering message", ex);
                return ServiceBusMessageStates.DeadLetter;
            }
        }
    }
}