namespace SFA.Apprenticeships.Web.Recruit.Converters
{
    using Domain.Entities.Providers;
    using ViewModels.Provider;

    public static class ProviderSiteEmployerLinkViewModelConverter
    {
        public static ProviderSiteEmployerLinkViewModel Convert(this ProviderSiteEmployerLink providerSiteEmployerLink)
        {
            var viewModel = new ProviderSiteEmployerLinkViewModel
            {
                ProviderSiteErn = providerSiteEmployerLink.ProviderSiteErn,
                Description = providerSiteEmployerLink.Description,
                WebsiteUrl = providerSiteEmployerLink.WebsiteUrl,
                IsWebsiteUrlWellFormed = providerSiteEmployerLink.IsWebsiteUrlWellFormed,
                Employer = providerSiteEmployerLink.Employer.Convert()
            };

            return viewModel;
        }
    }
}