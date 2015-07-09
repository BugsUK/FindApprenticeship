namespace SFA.Apprenticeships.Infrastructure.Processes.SiteMap
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Application.Vacancies.Entities.SiteMap;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using EasyNetQ.AutoSubscribe;

    public class CreateVacancySiteMapRequestConsumerAsync : IConsumeAsync<CreateVacancySiteMapRequest>
    {
        private readonly ILogService _logger;
        private readonly IConfigurationService _configurationService;
        private readonly ISiteMapVacancyProcessor _siteMapVacancyProcessor;

        public CreateVacancySiteMapRequestConsumerAsync(
            ILogService logger,
            IConfigurationService configurationService,
            ISiteMapVacancyProcessor siteMapVacancyProcessor)
        {
            _logger = logger;
            _configurationService = configurationService;
            _siteMapVacancyProcessor = siteMapVacancyProcessor;
        }

        // [SubscriptionConfiguration(PrefetchCount = 1)]
        // [AutoSubscriberConsumer(SubscriptionId = "CreateVacancySiteMapRequestConsumerAsync")]
        public Task Consume(CreateVacancySiteMapRequest request)
        {
            return Task.Run(() =>
            {
                if (!_configurationService.Get<ProcessConfiguration>().EnableVacancySiteMap)
                {
                    _logger.Info("Vacancy site map is currently disabled");
                    return;
                }

                _siteMapVacancyProcessor.Process(request);
            });
        }
    }
}