namespace SFA.Apprenticeships.Infrastructure.Processes.SiteMap
{
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Application.Vacancies.Entities.SiteMap;
    using Configuration;
    using Domain.Interfaces.Configuration;
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

        [ServiceBusTopicSubscription(TopicName = "create-vacancy-sitemap-request")]
        public ServiceBusMessageResult Consume(CreateVacancySiteMapRequest request)
        {
            if (_configurationService.Get<ProcessConfiguration>().EnableVacancySiteMap)
            {
                _siteMapVacancyProcessor.Process(request);
            }
            else
            {
                _logger.Info("Vacancy site map is currently disabled");
            }

            return ServiceBusMessageResult.Complete();
        }
    }
}