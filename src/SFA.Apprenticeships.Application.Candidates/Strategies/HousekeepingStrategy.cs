namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class HousekeepingStrategy : IHousekeepingStrategy
    {
        protected HousekeepingStrategy(IConfigurationService configurationService)
        {
            Configuration = configurationService.Get<HousekeepingConfiguration>();
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

        protected HousekeepingConfiguration Configuration { get; private set; }

        protected int GetHousekeepingCyclesSince(DateTime date)
        {
            var timeSince = DateTime.UtcNow - date;

            return (int)(timeSince.TotalHours / Configuration.HousekeepingCycleInHours);
        }

        protected abstract bool DoHandle(User user, Candidate candidate);
    }
}