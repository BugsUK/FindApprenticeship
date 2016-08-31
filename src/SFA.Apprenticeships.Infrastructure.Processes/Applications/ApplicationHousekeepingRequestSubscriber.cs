namespace SFA.Apprenticeships.Infrastructure.Processes.Applications
{
    using Application.Applications.Housekeeping;
    using Domain.Interfaces.Messaging;

    using SFA.Apprenticeships.Application.Interfaces;

    public class ApplicationHousekeepingRequestSubscriber : IServiceBusSubscriber<ApplicationHousekeepingRequest>
    {
        private readonly ILogService _logService;
        private readonly IRootApplicationHousekeeper _housekeeper;

        public ApplicationHousekeepingRequestSubscriber(
            ILogService logService,
            IRootApplicationHousekeeper housekeeper)
        {
            _logService = logService;
            _housekeeper = housekeeper;
        }

        [ServiceBusTopicSubscription(TopicName = "StartApplicationHousekeeping")]
        public ServiceBusMessageStates Consume(ApplicationHousekeepingRequest request)
        {
            _logService.Debug("Running housekeeping for application id {0} and vacancy type {1}",
                request.ApplicationId, request.VacancyType);

            _housekeeper.Handle(request);

            _logService.Debug("Housekeeping for application id {0} and vacancy type {1} complete",
                request.ApplicationId, request.VacancyType);

            return ServiceBusMessageStates.Complete;
        }
    }
}