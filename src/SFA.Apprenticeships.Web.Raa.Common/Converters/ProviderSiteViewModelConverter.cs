namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using ViewModels.Provider;

    public static class ProviderSiteViewModelConverter
    {
        public static ProviderSiteViewModel Convert(this ProviderSite providerSite)
        {
            var viewModel = new ProviderSiteViewModel
            {
                ProviderSiteId = providerSite.ProviderSiteId,
                EdsUrn = providerSite.EdsUrn,
                FullName = providerSite.FullName,
                TradingName = providerSite.TradingName,
                EmployerDescription = providerSite.EmployerDescription,
                CandidateDescription = providerSite.CandidateDescription,
                ContactDetailsForEmployer = providerSite.EmployerDescription,
                ContactDetailsForCandidate = providerSite.ContactDetailsForCandidate,
                Address = providerSite.Address.Convert(),
            };

            return viewModel;
        }
    }
}