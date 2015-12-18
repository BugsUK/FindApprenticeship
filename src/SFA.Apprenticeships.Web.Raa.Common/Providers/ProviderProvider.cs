using System;
using System.Collections.Generic;
using System.Linq;
using SFA.Infrastructure.Interfaces;
using SFA.Apprenticeships.Application.Interfaces.Employers;
using SFA.Apprenticeships.Application.Interfaces.Providers;
using SFA.Apprenticeships.Application.Interfaces.VacancyPosting;
using SFA.Apprenticeships.Domain.Entities.Providers;
using SFA.Apprenticeships.Web.Common.Converters;
using SFA.Apprenticeships.Web.Raa.Common.Configuration;
using SFA.Apprenticeships.Web.Raa.Common.Converters;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;

namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    public class ProviderProvider : IProviderProvider
    {
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IProviderService _providerService;
        private readonly IConfigurationService _configurationService;

        public ProviderProvider(IProviderService providerService, IConfigurationService configurationService, IVacancyPostingService vacancyPostingService)
        {
            _providerService = providerService;
            _configurationService = configurationService;
            _vacancyPostingService = vacancyPostingService;
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
            providerSiteEmployerLink.WebsiteUrl = viewModel.WebsiteUrl;
            providerSiteEmployerLink.Description = viewModel.Description;
            providerSiteEmployerLink = _providerService.SaveProviderSiteEmployerLink(providerSiteEmployerLink);

            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyGuid);
            if (vacancy != null)
            {
                vacancy.ProviderSiteEmployerLink = providerSiteEmployerLink;
                if (viewModel.IsEmployerLocationMainApprenticeshipLocation != null)
                    vacancy.IsEmployerLocationMainApprenticeshipLocation =
                        viewModel.IsEmployerLocationMainApprenticeshipLocation.Value;
                if (viewModel.NumberOfPositions != null) vacancy.NumberOfPositions = viewModel.NumberOfPositions.Value;

                _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);
            }

            var result = providerSiteEmployerLink.Convert();
            result.VacancyGuid = viewModel.VacancyGuid;
            result.IsEmployerLocationMainApprenticeshipLocation = viewModel.IsEmployerLocationMainApprenticeshipLocation;
            result.NumberOfPositions = viewModel.NumberOfPositions;

            return result;
        }

        public EmployerSearchViewModel GetProviderSiteEmployerLinkViewModels(string providerSiteErn)
        {
            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;
            var parameters = new EmployerSearchRequest(providerSiteErn);
            var providerSiteEmployerLinks = _providerService.GetProviderSiteEmployerLinks(parameters, 1, pageSize);
            var result = providerSiteEmployerLinks.ToViewModel(providerSiteEmployerLinks.Page.Select(psel => psel.Convert().Employer.ConvertToResult()));

            return new EmployerSearchViewModel
            {
                ProviderSiteErn = providerSiteErn,
                EmployerResultsPage = result
            };
        }

        public EmployerSearchViewModel GetProviderSiteEmployerLinkViewModels(EmployerSearchViewModel viewModel)
        {
            EmployerSearchRequest parameters;

            switch (viewModel.FilterType)
            {
                case EmployerFilterType.Ern:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteErn, viewModel.Ern);
                    break;
                case EmployerFilterType.NameAndLocation:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteErn, viewModel.Name, viewModel.Location);
                    break;
                case EmployerFilterType.Undefined:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteErn);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewModel), viewModel.FilterType, "");
            }

            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;

            var providerSiteEmployerLinks = _providerService.GetProviderSiteEmployerLinks(parameters, viewModel.EmployerResultsPage.CurrentPage, pageSize);

            var resultsViewModelPage = providerSiteEmployerLinks.ToViewModel(providerSiteEmployerLinks.Page.Select(e => e.Convert().Employer.ConvertToResult()));
            viewModel.EmployerResultsPage = resultsViewModelPage;
            return viewModel;
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