namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using Application.Vacancies;
    using Application.Vacancies.Entities;
    using Domain.Interfaces.Messaging;

    public class CandidateSavedSearchesSubscriber : IServiceBusSubscriber<CandidateSavedSearches>
    {
        private readonly ISavedSearchProcessor _savedSearchProcessor;

        public CandidateSavedSearchesSubscriber(ISavedSearchProcessor savedSearchProcessor)
        {
            _savedSearchProcessor = savedSearchProcessor;
        }

        [ServiceBusTopicSubscription(TopicName = "RunCandidateSavedSearches")]
        public ServiceBusMessageStates Consume(CandidateSavedSearches candidateSavedSearches)
        {
            _savedSearchProcessor.ProcessCandidateSavedSearches(candidateSavedSearches);

            return ServiceBusMessageStates.Complete;
        }
    }
}
