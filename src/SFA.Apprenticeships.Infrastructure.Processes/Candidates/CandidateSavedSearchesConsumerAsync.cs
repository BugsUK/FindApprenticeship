namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using System.Threading.Tasks;
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using EasyNetQ.AutoSubscribe;

    public class CandidateSavedSearchesConsumerAsync : IConsumeAsync<CandidateSavedSearches>
    {
        private readonly ISavedSearchProcessor _savedSearchProcessor;

        public CandidateSavedSearchesConsumerAsync(ISavedSearchProcessor savedSearchProcessor)
        {
            _savedSearchProcessor = savedSearchProcessor;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "CandidateSavedSearchesConsumerAsync")]
        public Task Consume(CandidateSavedSearches candidateSavedSearches)
        {
            return Task.Run(() => _savedSearchProcessor.ProcessCandidateSavedSearches(candidateSavedSearches));
        }
    }
}
