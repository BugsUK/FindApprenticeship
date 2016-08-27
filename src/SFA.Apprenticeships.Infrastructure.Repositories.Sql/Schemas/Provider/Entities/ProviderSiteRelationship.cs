namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities
{
    public class ProviderSiteRelationship
    {
        public int ProviderSiteRelationshipID { get; set; }
        public int ProviderID { get; set; }
        public int ProviderSiteID { get; set; }
        public int ProviderSiteRelationShipTypeID { get; set; }
    }
}