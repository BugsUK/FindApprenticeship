using System.Collections.Generic;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Linq;
    using Application.Interfaces.Providers;
    using Converters;
    using Domain.Entities.Providers;

    public class ProviderProvider : IProviderProvider
    {
        private readonly IProviderService _providerService;

        public ProviderProvider(IProviderService providerService)
        {
            _providerService = providerService;
        }

        public ProviderViewModel GetProviderViewModel(string ukprn)
        {
            var provider = _providerService.GetProvider(ukprn);
            var providerSites = _providerService.GetProviderSites(ukprn);

            return Convert(provider, providerSites);
        }

        public ProviderViewModel SaveProviderViewModel(string ukprn, ProviderViewModel providerViewModel)
        {
            var provider = _providerService.GetProvider(ukprn);
            var providerSites = _providerService.GetProviderSites(ukprn);

            //TODO: Combine existing with anything that can be updated from the passed view model

            _providerService.SaveProvider(provider);
            _providerService.SaveProviderSites(providerSites);

            return GetProviderViewModel(ukprn);
        }

        public ProviderSiteViewModel GetProviderSiteViewModel(string ukprn, string ern)
        {
            var providerSite = _providerService.GetProviderSite(ukprn, ern);

            return providerSite.Convert();
        }

        public IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn)
        {
            var providerSites = _providerService.GetProviderSites(ukprn);
            return providerSites.Select(ps => ps.Convert());
        }

        public ProviderSiteEmployerLinkViewModel GetProviderSiteEmployerLinkViewModel(string providerSiteErn, string ern)
        {
            var providerSiteEmployerLink = _providerService.GetProviderSiteEmployerLink(providerSiteErn, ern);
            return providerSiteEmployerLink.Convert();
        }

        public ProviderSiteEmployerLinkViewModel ConfirmProviderSiteEmployerLink(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var providerSiteEmployerLink = _providerService.GetProviderSiteEmployerLink(viewModel.ProviderSiteErn, viewModel.Employer.Ern);
            providerSiteEmployerLink.Description = viewModel.Description;
            providerSiteEmployerLink = _providerService.SaveProviderSiteEmployerLink(providerSiteEmployerLink);
            return providerSiteEmployerLink.Convert();
        }

        public IEnumerable<ProviderSiteEmployerLinkViewModel> GetProviderSiteEmployerLinkViewModels(string providerSiteErn)
        {
            var providerSiteEmployerLinks = _providerService.GetProviderSiteEmployerLinks(providerSiteErn);
            return providerSiteEmployerLinks.Select(psel => psel.Convert());
        }

        private static ProviderViewModel Convert(Provider provider, IEnumerable<ProviderSite> providerSites)
        {
            var viewModel = new ProviderViewModel
            {
                ProviderName = provider.Name,
                ProviderSiteViewModels = providerSites.Select(ps => ps.Convert()).ToList()
            };

            return viewModel;
        }
    }
}