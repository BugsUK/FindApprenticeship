namespace SFA.Apprenticeships.Data.Migrate.Faa.Subscribers
{
    using System;
    using Application.Application.Entities;
    using Application.Interfaces;
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
            _logService.Debug($"Updating apprenticeship application with id {request.ApplicationGuid}");

            try
            {
                _apprenticeshipApplicationUpdater.Update(request.ApplicationGuid);

                _logService.Debug($"Updated apprenticeship application with id {request.ApplicationGuid}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to update apprenticeship application with id {request.ApplicationGuid}", ex);
            }

            return ServiceBusMessageStates.Complete;
        }
    }
}