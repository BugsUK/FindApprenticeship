namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using ViewModels.Provider;

    public static class ProviderSiteRelationshipViewModelConverter
    {
        public static ProviderSiteRelationshipViewModel Convert(this ProviderSiteRelationship providerSiteRelationship)
        {
            var viewModel = new ProviderSiteRelationshipViewModel
            {
                ProviderSiteRelationshipId = providerSiteRelationship.ProviderSiteRelationshipId,
                ProviderId = providerSiteRelationship.ProviderId,
                ProviderSiteId = providerSiteRelationship.ProviderSiteId,
                ProviderSiteRelationshipType = providerSiteRelationship.ProviderSiteRelationShipTypeId,
                ProviderUkprn = providerSiteRelationship.ProviderUkprn,
                ProviderFullName = providerSiteRelationship.ProviderFullName,
                ProviderTradingName = providerSiteRelationship.ProviderTradingName,
                ProviderSiteFullName = providerSiteRelationship.ProviderSiteFullName,
                ProviderSiteTradingName = providerSiteRelationship.ProviderSiteTradingName
            };

            return viewModel;
        }
    }
}