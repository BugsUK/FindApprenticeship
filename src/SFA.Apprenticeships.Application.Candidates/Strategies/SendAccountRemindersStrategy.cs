namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Interfaces.Logging;

    public class SendAccountRemindersStrategy : HousekeepingStrategy
    {
        private readonly ILogService _logService;

        public SendAccountRemindersStrategy(IConfigurationService configurationService, ILogService logService)
            : base(configurationService)
        {
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            throw new NotImplementedException();
        }
    }
}