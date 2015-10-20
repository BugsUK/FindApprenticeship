namespace SFA.Apprenticeships.Web.Recruit.Converters
{
    using System;
    using Domain.Entities.Providers;
    using ViewModels.Provider;

    public static class ProviderSiteEmployerLinkViewModelConverter
    {
        public static ProviderSiteEmployerLinkViewModel Convert(this ProviderSiteEmployerLink providerSiteEmployerLink)
        {
            string websiteUrl;
            var isWebsiteUrlWellFormed = TryParseWebsiteUrl(providerSiteEmployerLink.WebsiteUrl, out websiteUrl);

            var viewModel = new ProviderSiteEmployerLinkViewModel
            {
                ProviderSiteErn = providerSiteEmployerLink.ProviderSiteErn,
                Description = providerSiteEmployerLink.Description,
                WebsiteUrl = websiteUrl,
                IsWebsiteUrlWellFormed = isWebsiteUrlWellFormed,
                Employer = providerSiteEmployerLink.Employer.Convert()
            };

            return viewModel;
        }

        private static bool TryParseWebsiteUrl(string website, out string websiteUrl)
        {
            websiteUrl = website;
            if (IsValidUrl(website))
            {
                websiteUrl = new UriBuilder(website).Uri.ToString();
                return true;
            }
            return false;
        }

        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            try
            {
                // Attempting to build the URL will throw an exception if it is invalid.
                // ReSharper disable once UnusedVariable
                var unused = new UriBuilder(url);

                return true;
            }
            catch (UriFormatException)
            {
                return false;
            }
        }
    }
}