namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using System.Threading.Tasks;
    using Application.Applications.Housekeeping;
    using Application.Interfaces.Logging;
    using EasyNetQ.AutoSubscribe;

    public class ApplicationHousekeepingRequestConsumerAsync : IConsumeAsync<ApplicationHousekeepingRequest>
    {
        private readonly ILogService _logService;
        private readonly IRootApplicationHousekeeper _housekeeper;

        public ApplicationHousekeepingRequestConsumerAsync(
            ILogService logService,
            IRootApplicationHousekeeper housekeeper)
        {
            _logService = logService;
            _housekeeper = housekeeper;
        }

        // [SubscriptionConfiguration(PrefetchCount = 20)]
        // [AutoSubscriberConsumer(SubscriptionId = "ApplicationHousekeepingRequestConsumerAsync")]
        public Task Consume(ApplicationHousekeepingRequest request)
        {
            return Task.Run(() =>
            {
                _logService.Debug("Running housekeeping for application id {0} and vacancy type {1}",
                    request.ApplicationId, request.VacancyType);

                _housekeeper.Handle(request);

                _logService.Debug("Housekeeping for application id {0} and vacancy type {1} complete",
                    request.ApplicationId, request.VacancyType);
            });
        }
    }
}