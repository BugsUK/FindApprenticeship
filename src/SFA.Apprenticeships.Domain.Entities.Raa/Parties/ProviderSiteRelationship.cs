namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    public class ProviderSiteRelationship
    {
        public int ProviderSiteRelationshipId { get; set; }
        public int ProviderId { get; set; }
        public int ProviderSiteId { get; set; }
        public int ProviderSiteRelationShipTypeId { get; set; }
    }
}