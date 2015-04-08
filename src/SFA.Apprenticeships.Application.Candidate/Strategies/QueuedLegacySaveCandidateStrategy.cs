namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;

    public class QueuedLegacySaveCandidateStrategy : ISaveCandidateStrategy
    {
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;
        private readonly IMessageBus _messageBus;

        public QueuedLegacySaveCandidateStrategy(ISaveCandidateStrategy saveCandidateStrategy, IMessageBus messageBus)
        {
            _saveCandidateStrategy = saveCandidateStrategy;
            _messageBus = messageBus;
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            var savedCandidate = _saveCandidateStrategy.SaveCandidate(candidate);

            var request = new SaveCandidateRequest { CandidateId = savedCandidate.EntityId };
            _messageBus.PublishMessage(request);

            return savedCandidate;
        }
    }
}