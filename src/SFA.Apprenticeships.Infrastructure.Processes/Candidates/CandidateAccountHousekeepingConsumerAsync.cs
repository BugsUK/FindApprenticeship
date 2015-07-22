namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Candidates;
    using Application.Candidates.Entities;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;

    public class CandidateAccountHousekeepingConsumerAsync : IConsumeAsync<CandidateHousekeeping>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IList<IHousekeepingChainOfResponsibility> _housekeepingChains;
        private readonly ILogService _logService;

        public CandidateAccountHousekeepingConsumerAsync(IUserReadRepository userReadRepository, ICandidateReadRepository candidateReadRepository, IEnumerable<IHousekeepingChainOfResponsibility> housekeepingChains, ILogService logService)
        {
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _housekeepingChains = housekeepingChains.OrderBy(hc => hc.Order).ToList();
            _logService = logService;
        }

        // [SubscriptionConfiguration(PrefetchCount = 20)]
        // [AutoSubscriberConsumer(SubscriptionId = "CandidateAccountHousekeepingConsumerAsync")]
        public Task Consume(CandidateHousekeeping candidateHousekeeping)
        {
            return Task.Run(() =>
            {
                var candidateId = candidateHousekeeping.CandidateId;
                _logService.Debug("Running housekeeping strategies for CandidateId: {0}", candidateId);
                var user = _userReadRepository.Get(candidateId);
                var candidate = _candidateReadRepository.Get(candidateId);
                if (user == null && candidate == null)
                {
                    _logService.Info("No User or Candidate found for CandidateId: {0}. Likely deleted by another housekeeping sweep", candidateId);
                }
                else
                {
                    foreach (var housekeepingChain in _housekeepingChains)
                    {
                        housekeepingChain.Handle(user, candidate);
                    }
                }
                _logService.Debug("Housekeeping for CandidateId: {0} complete", candidateId);
            });
        }
    }
}