namespace SFA.Apprenticeships.Infrastructure.Processes.SiteMap
{
    using SFA.Infrastructure.Interfaces;
    using Application.Vacancies;
    using Application.Vacancies.Entities.SiteMap;
    using Configuration;
    using Domain.Interfaces.Messaging;

    public class CreateVacancySiteMapRequestSubscriber : IServiceBusSubscriber<CreateVacancySiteMapRequest>
    {
        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;
        private readonly ISiteMapVacancyProcessor _siteMapVacancyProcessor;

        public CreateVacancySiteMapRequestSubscriber(
            ILogService logger,
            IConfigurationService configurationService,
            ISiteMapVacancyProcessor siteMapVacancyProcessor)
        {
            _logger = logger;
            _configurationService = configurationService;
            _siteMapVacancyProcessor = siteMapVacancyProcessor;
        }

        [ServiceBusTopicSubscription(TopicName = "CreateVacancySiteMap")]
        public ServiceBusMessageStates Consume(CreateVacancySiteMapRequest request)
        {
            if (_configurationService.Get<ProcessConfiguration>().EnableVacancySiteMap)
            {
                _siteMapVacancyProcessor.Process(request);
            }
            else
            {
                _logger.Info("Vacancy site map is currently disabled");
            }

            return ServiceBusMessageStates.Complete;
        }
    }
}