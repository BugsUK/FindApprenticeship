namespace SFA.Apprenticeships.Infrastructure.Processes.Candidates
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidates;
    using Application.Candidates.Entities;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;

    public class CandidateAccountHousekeepingSubscriber : IServiceBusSubscriber<CandidateHousekeeping>
    {
        private readonly ILogService _logService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IList<IHousekeepingChainOfResponsibility> _housekeepingChains;

        public CandidateAccountHousekeepingSubscriber(
            ILogService logService,
            IUserReadRepository userReadRepository,
            ICandidateReadRepository candidateReadRepository,
            IEnumerable<IHousekeepingChainOfResponsibility> housekeepingChains)
        {
            _logService = logService;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _housekeepingChains = housekeepingChains.OrderBy(hc => hc.Order).ToList();
        }

        [ServiceBusTopicSubscription(TopicName = "StartCandidateHousekeeping")]
        public ServiceBusMessageStates Consume(CandidateHousekeeping candidateHousekeeping)
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

            return ServiceBusMessageStates.Complete;
        }
    }
}