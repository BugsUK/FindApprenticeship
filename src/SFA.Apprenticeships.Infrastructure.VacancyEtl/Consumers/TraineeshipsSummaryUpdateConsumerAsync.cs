namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    public class TraineeshipsSummaryUpdateConsumerAsync : IConsumeAsync<TraineeshipSummaryUpdate>
    {
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> _vacancyIndexerService;

        public TraineeshipsSummaryUpdateConsumerAsync(IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary> vacancyIndexerService)
        {
            _vacancyIndexerService = vacancyIndexerService;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "TraineeshipsSummaryUpdateConsumerAsync")]
        public Task Consume(TraineeshipSummaryUpdate vacancySummaryToIndex)
        {
            return Task.Run(() => _vacancyIndexerService.Index(vacancySummaryToIndex));
        }
    }
}
