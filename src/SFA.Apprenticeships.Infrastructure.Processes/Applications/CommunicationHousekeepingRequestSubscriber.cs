﻿namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using Application.Communications.Housekeeping;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;

    public class CommunicationHousekeepingRequestSubscriber : IServiceBusSubscriber<CommunicationHousekeepingRequest>
    {
        private readonly ILogService _logService;
        private readonly IRootCommunicationHousekeeper _housekeeper;

        public CommunicationHousekeepingRequestSubscriber(
            ILogService logService,
            IRootCommunicationHousekeeper housekeeper)
        {
            _logService = logService;
            _housekeeper = housekeeper;
        }

        [ServiceBusTopicSubscription(TopicName = "StartCommunicationHousekeeping")]
        public ServiceBusMessageResult Consume(CommunicationHousekeepingRequest request)
        {
            _logService.Debug("Running housekeeping for communication id {0} and type {1}",
                request.CommunicationId, request.CommunicationType);

            _housekeeper.Handle(request);

            _logService.Debug("Housekeeping for communication id {0} and type {1} complete",
                request.CommunicationId, request.CommunicationType);

            return ServiceBusMessageResult.Complete();
        }
    }
}