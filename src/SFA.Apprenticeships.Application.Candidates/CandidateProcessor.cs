namespace SFA.Apprenticeships.Application.Candidates
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Interfaces.Logging;

    public class CandidateProcessor : ICandidateProcessor
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMessageBus _messageBus;
        private readonly ILogService _logService;

        public CandidateProcessor(IUserReadRepository userReadRepository, IMessageBus messageBus, ILogService logService)
        {
            _userReadRepository = userReadRepository;
            _messageBus = messageBus;
            _logService = logService;
        }

        public void QueueCandidates()
        {
            var users = _userReadRepository.GetUsersWithStatus(new[] { UserStatuses.PendingActivation });
            
            var counter = 0;
            Parallel.ForEach(users, user =>
            {
                var candidateHousekeeping = new CandidateHousekeeping
                {
                    CandidateId = user.EntityId
                };
                _messageBus.PublishMessage(candidateHousekeeping);
                Interlocked.Increment(ref counter);
            });

            _logService.Debug("Queued {0} candidates for Housekeeping", counter);
        }
    }
}