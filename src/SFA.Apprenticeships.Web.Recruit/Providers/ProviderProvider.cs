using System.Collections.Generic;
using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Apprenticeships.Domain.Interfaces.Configuration;
using SFA.Apprenticeships.Web.Common.Converters;
using SFA.Apprenticeships.Web.Recruit.Configuration;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Provider;
using SFA.Apprenticeships.Web.Recruit.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System.Linq;
    using Application.Interfaces.Providers;
    using Converters;
    using Domain.Entities.Providers;

    public class ProviderProvider : IProviderProvider
    {
        private readonly IProviderService _providerService;
        private readonly IConfigurationService _configurationService;

        public ProviderProvider(IProviderService providerService, IConfigurationService configurationService)
        {
            _providerService = providerService;
            _configurationService = configurationService;
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
            var pageSize = int.Parse(_configurationService.Get<RecruitWebConfiguration>().PageSize);
            var parameters = new EmployerSearchRequest(providerSiteErn);
            var providerSiteEmployerLinks = _providerService.GetProviderSiteEmployerLinks(parameters, 0, pageSize);
            var result = providerSiteEmployerLinks.Page.Select(psel => psel.Convert());
            return result;
        }

        public IEnumerable<ProviderSiteEmployerLinkViewModel> GetProviderSiteEmployerLinkViewModels(EmployerSearchViewModel viewModel)
        {
            EmployerSearchRequest parameters = new EmployerSearchRequest(viewModel.ProviderSiteErn);

            switch (viewModel.FilterType)
            {
                case EmployerFilterType.Ern:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteErn, viewModel.Ern);
                    break;
                case EmployerFilterType.NameAndLocation:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteErn, viewModel.Name, viewModel.Location);
                    break;
            }

            var pageSize = int.Parse(_configurationService.Get<RecruitWebConfiguration>().PageSize);

            var providerSiteEmployerLinks = _providerService.GetProviderSiteEmployerLinks(parameters, viewModel.EmployerResultsPage.CurrentPage, pageSize);

            var resultsViewModelPage = providerSiteEmployerLinks.Page.Select(e => e.Convert()).ToList();
            return resultsViewModelPage;
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