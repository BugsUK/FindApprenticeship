namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Consumers
{
    using System;
    using System.Threading.Tasks;
    using Application.Vacancies.Entities;
    using EasyNetQ.AutoSubscribe;

    public class CandidateSavedSearchesConsumerAsync : IConsumeAsync<CandidateSavedSearches>
    {
        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "CandidateSavedSearchesConsumerAsync")]
        public Task Consume(CandidateSavedSearches candidateSavedSearches)
        {
            return Task.Run(() =>
            {
                //todo: 1.8: call ISavedSearchProcessor.ProcessCandidateSavedSearches to execute saved searches for a candidate
            });
        }
    }
}
