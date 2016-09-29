namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using Domain.Entities.Raa.Parties;

    public class ProviderSiteRelationshipViewModel
    {
        public int ProviderSiteRelationshipId { get; set; }
        public int ProviderId { get; set; }
        public int ProviderSiteId { get; set; }
        public ProviderSiteRelationshipTypes ProviderSiteRelationshipType { get; set; }
    }
}