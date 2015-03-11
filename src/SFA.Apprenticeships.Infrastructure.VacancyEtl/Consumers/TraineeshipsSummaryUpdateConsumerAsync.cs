namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Vacancies.Entities;
    using EasyNetQ.AutoSubscribe;
    using Elastic.Common.Entities;
    using VacancyIndexer;

    //todo: 1.8: move to async processor
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
