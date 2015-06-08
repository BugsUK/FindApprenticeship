namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System.Threading.Tasks;
    using Application.Communications.Housekeeping;
    using Application.Interfaces.Logging;
    using EasyNetQ.AutoSubscribe;

    public class CommunicationHousekeepingRequestConsumerAsync : IConsumeAsync<CommunicationHousekeepingRequest>
    {
        private readonly ILogService _logService;
        private readonly IRootCommunicationHousekeeper _housekeeper;

        public CommunicationHousekeepingRequestConsumerAsync(
            ILogService logService,
            IRootCommunicationHousekeeper housekeeper)
        {
            _logService = logService;
            _housekeeper = housekeeper;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "CommunicationHousekeepingRequestConsumerAsync")]
        public Task Consume(CommunicationHousekeepingRequest request)
        {
            return Task.Run(() =>
            {
                _logService.Debug("Running housekeeping for communication id {0} and type {1}",
                    request.CommunicationId, request.CommunicationType);

                _housekeeper.Handle(request);

                _logService.Debug("Housekeeping for communication id {0} and type {1} complete",
                    request.CommunicationId, request.CommunicationType);
            });
        }
    }
}