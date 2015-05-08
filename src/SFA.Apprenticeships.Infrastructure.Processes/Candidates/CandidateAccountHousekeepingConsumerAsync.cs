namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using System.Threading.Tasks;
    using Application.Candidates.Entities;
    using Application.Candidates.Strategies;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;

    public class CandidateAccountHousekeepingConsumerAsync : IConsumeAsync<CandidateHousekeeping>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IHousekeepingStrategy _strategy;
        private readonly ILogService _logService;

        public CandidateAccountHousekeepingConsumerAsync(IUserReadRepository userReadRepository, ICandidateReadRepository candidateReadRepository, IHousekeepingStrategy strategy, ILogService logService)
        {
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _strategy = strategy;
            _logService = logService;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "CandidateAccountHousekeepingConsumerAsync")]
        public Task Consume(CandidateHousekeeping candidateHousekeeping)
        {
            return Task.Run(() =>
            {
                var candidateId = candidateHousekeeping.CandidateId;
                _logService.Debug("Running housekeeping strategies for CandidateId: {0}", candidateId);
                var user = _userReadRepository.Get(candidateId);
                var candidate = _candidateReadRepository.Get(candidateId);
                if (user == null || candidate == null)
                {
                    _logService.Warn("Housekeeping for CandidateId: {0} found a null user {1} or candidate {2}", candidateId, user == null, candidate == null);
                }
                else
                {
                    _strategy.Handle(user, candidate);
                }
                _logService.Debug("Housekeeping for CandidateId: {0} complete", candidateId);
            });
        }
    }
}