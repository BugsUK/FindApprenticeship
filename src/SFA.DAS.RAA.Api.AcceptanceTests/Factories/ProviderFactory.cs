namespace SFA.DAS.RAA.Api.AcceptanceTests.Factories
{
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities;

    public class ProviderFactory
    {
        public static Provider GetSkillsFundingAgencyProvider()
        {
            return new Provider
            {
                ProviderId = 1170,
                FullName = "CHIEF EXECUTIVE OF SKILLS FUNDING",
                TradingName = "Department for Business, Innovation and Skills - Skills Funding Agency"
            };
        }
    }
}