namespace SFA.Apprenticeships.Infrastructure.Processes.SiteMap
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;
    using Application.Vacancies;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using EasyNetQ.AutoSubscribe;
    using Web.Common.SiteMap;

    // TODO: AG: US438: logging.
    public class CreateVacancySiteMapRequestConsumerAsync : IConsumeAsync<CreateVacancySiteMapRequest>
    {
        private readonly ILogService _logger;
        private readonly IVacancySiteMapProcessor _vacancySiteMapProcessor;
        private readonly bool _enableVacancyStatusPropagation;

        public CreateVacancySiteMapRequestConsumerAsync(
            ILogService logger,
            IConfigurationService configurationService,
            IVacancySiteMapProcessor vacancySiteMapProcessor)
        {
            _logger = logger;
            _vacancySiteMapProcessor = vacancySiteMapProcessor;
            _enableVacancyStatusPropagation = configurationService.Get<ProcessConfiguration>().EnableVacancyStatusPropagation;
        }

        [SubscriptionConfiguration(PrefetchCount = 1)]
        [AutoSubscriberConsumer(SubscriptionId = "CreateVacancySiteMapRequestConsumerAsync")]
        public Task Consume(CreateVacancySiteMapRequest request)
        {
            return Task.Run(() =>
            {
                if (!_enableVacancyStatusPropagation)
                {
                    // TODO: AG: US438: logging.
                    return;
                }

                _vacancySiteMapProcessor.CreateVacancySiteMap(request);
            });
        }
    }
}