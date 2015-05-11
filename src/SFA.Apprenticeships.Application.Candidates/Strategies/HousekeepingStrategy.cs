namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;

    public abstract class HousekeepingStrategy : IHousekeepingStrategy
    {
        private readonly IConfigurationService _configurationService;

        protected HousekeepingStrategy(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public IHousekeepingStrategy Successor { get; private set; }

        public void SetSuccessor(IHousekeepingStrategy successor)
        {
            Successor = successor;
        }

        public void Handle(User user, Candidate candidate)
        {
            if (DoHandle(user, candidate)) return;

            Successor.Handle(user, candidate);
        }

        protected HousekeepingConfiguration Configuration
        {
            get { return _configurationService.Get<HousekeepingConfiguration>(); }
        }

        protected int GetHousekeepingCyclesSinceCreation(User user)
        {
            var timeSinceCreation = DateTime.UtcNow - user.DateCreated.ToUniversalTime();

            var housekeepingCyclesSinceCreation = (int)(timeSinceCreation.TotalHours / Configuration.HousekeepingCycleInHours);

            return housekeepingCyclesSinceCreation;
        }

        protected abstract bool DoHandle(User user, Candidate candidate);
    }
}