namespace SFA.Apprenticeships.Infrastructure.Processes.Vacancies
{
    using System;
    using System.Threading.Tasks;
    using Application.Vacancies.Entities;
    using EasyNetQ.AutoSubscribe;
    using VacancyIndexer;
    using Elastic = Elastic.Common.Entities;

    public class TraineeshipsSummaryUpdateConsumerAsync : IConsumeAsync<TraineeshipSummaryUpdate>
    {
        private readonly IVacancyIndexerService<TraineeshipSummaryUpdate, Elastic.TraineeshipSummary> _vacancyIndexerService;

        public TraineeshipsSummaryUpdateConsumerAsync(IVacancyIndexerService<TraineeshipSummaryUpdate, Elastic.TraineeshipSummary> vacancyIndexerService)
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
