namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;

    public class QueuedLegacySaveCandidateStrategy : ISaveCandidateStrategy
    {
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;
        private readonly IMessageBus _messageBus;
        private readonly IServiceBus _serviceBus;

        public QueuedLegacySaveCandidateStrategy(
            ISaveCandidateStrategy saveCandidateStrategy,
            IMessageBus messageBus,
            IServiceBus serviceBus)
        {
            _saveCandidateStrategy = saveCandidateStrategy;
            _messageBus = messageBus;
            _serviceBus = serviceBus;
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            var savedCandidate = _saveCandidateStrategy.SaveCandidate(candidate);

            if (savedCandidate.LegacyCandidateId > 0)
            {
                var request = new SaveCandidateRequest
                {
                    CandidateId = savedCandidate.EntityId
                };

                _messageBus.PublishMessage(request);
                // TODO: AG.
                _serviceBus.PublishMessage(request);
            }

            return savedCandidate;
        }
    }
}