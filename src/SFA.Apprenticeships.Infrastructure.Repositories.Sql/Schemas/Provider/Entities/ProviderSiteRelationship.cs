namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities
{
    public class ProviderSiteRelationship
    {
        public int ProviderSiteRelationshipID { get; set; }
        public int ProviderID { get; set; }
        public int ProviderSiteID { get; set; }
        public int ProviderSiteRelationShipTypeID { get; set; }
        public string ProviderUkprn { get; set; }
        public string ProviderFullName { get; set; }
        public string ProviderTradingName { get; set; }
    }
}