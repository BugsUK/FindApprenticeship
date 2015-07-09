namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Messaging;

    public class QueuedLegacySaveCandidateStrategy : ISaveCandidateStrategy
    {
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;
        private readonly IServiceBus _serviceBus;

        public QueuedLegacySaveCandidateStrategy(
            ISaveCandidateStrategy saveCandidateStrategy,
            IServiceBus serviceBus)
        {
            _saveCandidateStrategy = saveCandidateStrategy;
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

                _serviceBus.PublishMessage(request);
            }

            return savedCandidate;
        }
    }
}