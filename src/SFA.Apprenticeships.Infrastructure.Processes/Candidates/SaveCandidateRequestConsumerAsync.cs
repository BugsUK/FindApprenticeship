namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using System;
    using System.Threading.Tasks;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;

    public class SaveCandidateRequestConsumerAsync : IConsumeAsync<SaveCandidateRequest>
    {
        private readonly IMessageBus _messageBus;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly ILogService _logger;

        public SaveCandidateRequestConsumerAsync(IMessageBus messageBus, ICandidateReadRepository candidateReadRepository, ILegacyCandidateProvider legacyCandidateProvider, ILogService logger)
        {
            _messageBus = messageBus;
            _candidateReadRepository = candidateReadRepository;
            _legacyCandidateProvider = legacyCandidateProvider;
            _logger = logger;
        }

        [SubscriptionConfiguration(PrefetchCount = 2)]
        [AutoSubscriberConsumer(SubscriptionId = "SaveCandidateRequestConsumerAsync")]
        public Task Consume(SaveCandidateRequest request)
        {
            return Task.Run(() =>
            {
                if (request.ProcessTime.HasValue && request.ProcessTime > DateTime.Now)
                {
                    try
                    {
                        _messageBus.PublishMessage(request);
                        return;
                    }
                    catch
                    {
                        _logger.Error("Failed to re-queue deferred 'Create Candidate' request: {{ 'CandidateId': '{0}' }}", request.CandidateId);
                        throw;
                    }
                }

                SaveCandidate(request);
            });
        }

        private void SaveCandidate(SaveCandidateRequest request)
        {
            var candidate = _candidateReadRepository.Get(request.CandidateId);
            
            _legacyCandidateProvider.UpdateCandidate(candidate);
        }
    }
}