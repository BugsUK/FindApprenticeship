namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Interfaces.Logging;

    public class SetPendingDeletionStrategy : HousekeepingStrategy
    {
        private readonly ILogService _logService;

        public SetPendingDeletionStrategy(IConfigurationService configurationService, ILogService logService)
            : base(configurationService)
        {
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            throw new System.NotImplementedException();
        }
    }
}