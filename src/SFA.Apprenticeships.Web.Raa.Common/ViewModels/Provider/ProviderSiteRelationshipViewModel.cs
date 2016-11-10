namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using Domain.Entities.Raa.Parties;

    public class ProviderSiteRelationshipViewModel
    {
        public int ProviderSiteRelationshipId { get; set; }
        public int ProviderId { get; set; }
        public int ProviderSiteId { get; set; }
        public ProviderSiteRelationshipTypes ProviderSiteRelationshipType { get; set; }
        public string ProviderUkprn { get; set; }
        public string ProviderFullName { get; set; }
        public string ProviderTradingName { get; set; }
        public string ProviderSiteFullName { get; set; }
        public string ProviderSiteTradingName { get; set; }
    }
}