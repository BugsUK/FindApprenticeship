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
                Ern = providerSite.EdsErn,
                Name = providerSite.Name,
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