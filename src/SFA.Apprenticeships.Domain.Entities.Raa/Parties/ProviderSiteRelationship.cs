namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    public class ProviderSiteRelationship
    {
        public int ProviderSiteRelationshipId { get; set; }
        public int ProviderId { get; set; }
        public int ProviderSiteId { get; set; }
        public ProviderSiteRelationshipTypes ProviderSiteRelationShipTypeId { get; set; }
        public string ProviderUkprn { get; set; }
        public string ProviderFullName { get; set; }
        public string ProviderTradingName { get; set; }
        public string ProviderSiteFullName { get; set; }
        public string ProviderSiteTradingName { get; set; }
    }
}