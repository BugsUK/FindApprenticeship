namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities
{
    using Dapper.Contrib.Extensions;

    public class ProviderSiteRelationship
    {
        public int ProviderSiteRelationshipId { get; set; }
        public int ProviderId { get; set; }
        public int ProviderSiteId { get; set; }
        public int ProviderSiteRelationShipTypeId { get; set; }
        [Write(false)]
        public string ProviderUkprn { get; set; }
        [Write(false)]
        public string ProviderFullName { get; set; }
        [Write(false)]
        public string ProviderTradingName { get; set; }
    }
}