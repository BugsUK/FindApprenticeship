namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;

    public class TerminatingHousekeepingStrategy : HousekeepingStrategy
    {
        public TerminatingHousekeepingStrategy(IConfigurationService configurationService) : base(configurationService)
        {
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            //Consume
            return true;
        }
    }
}