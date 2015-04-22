namespace SFA.Apprenticeships.Infrastructure.Processes.SiteMap
{
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using EasyNetQ.AutoSubscribe;
    using Web.Common.SiteMap;

    public class CreateVacancySiteMapRequestConsumerAsync : IConsumeAsync<CreateVacancySiteMapRequest>
    {
        private readonly ILogService _logger;
        private readonly IVacancySiteMapProcessor _vacancySiteMapProcessor;
        private readonly bool _enableVacancySiteMap;

        public CreateVacancySiteMapRequestConsumerAsync(
            ILogService logger,
            IConfigurationService configurationService,
            IVacancySiteMapProcessor vacancySiteMapProcessor)
        {
            _logger = logger;
            _vacancySiteMapProcessor = vacancySiteMapProcessor;
            _enableVacancySiteMap = configurationService.Get<ProcessConfiguration>().EnableVacancySiteMap;
        }

        [SubscriptionConfiguration(PrefetchCount = 1)]
        [AutoSubscriberConsumer(SubscriptionId = "CreateVacancySiteMapRequestConsumerAsync")]
        public Task Consume(CreateVacancySiteMapRequest request)
        {
            return Task.Run(() =>
            {
                if (!_enableVacancySiteMap)
                {
                    _logger.Info("Vacancy site map is currently disabled");
                    return;
                }

                _vacancySiteMapProcessor.Process(request);
            });
        }
    }
}