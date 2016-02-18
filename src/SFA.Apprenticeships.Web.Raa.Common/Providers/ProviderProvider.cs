namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Configuration;
    using Converters;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Provider;
    using ViewModels.VacancyPosting;
    using Web.Common.Converters;

    public class ProviderProvider : IProviderProvider, IProviderQAProvider
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

        public ProviderSiteEmployerLinkViewModel GetProviderSiteEmployerLinkViewModel(int providerSiteId, int employerId)
        {
            var providerSiteEmployerLink = _providerService.GetVacancyParty(providerSiteId, employerId);
            return providerSiteEmployerLink.Convert();
        }

        public ProviderSiteEmployerLinkViewModel ConfirmProviderSiteEmployerLink(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var providerSiteEmployerLink = _providerService.GetVacancyParty(viewModel.ProviderSiteId, viewModel.EmployerId);
            providerSiteEmployerLink.EmployerWebsiteUrl = viewModel.WebsiteUrl;
            providerSiteEmployerLink.EmployerDescription = viewModel.Description;
            providerSiteEmployerLink = _providerService.SaveVacancyParty(providerSiteEmployerLink);

            var vacancy = GetVacancy(viewModel);
            if (vacancy != null)
            {
                vacancy.OwnerPartyId = providerSiteEmployerLink.VacancyPartyId;
                vacancy.EmployerWebsiteUrl = providerSiteEmployerLink.EmployerWebsiteUrl;
                vacancy.EmployerDescription = providerSiteEmployerLink.EmployerDescription;
                if (viewModel.IsEmployerLocationMainApprenticeshipLocation != null)
                    vacancy.IsEmployerLocationMainApprenticeshipLocation =
                        viewModel.IsEmployerLocationMainApprenticeshipLocation.Value;
                if (viewModel.NumberOfPositions != null) vacancy.NumberOfPositions = viewModel.NumberOfPositions.Value;

                _vacancyPostingService.SaveApprenticeshipVacancy(vacancy);
            }

            var result = providerSiteEmployerLink.Convert();
            
            return result;
        }

        private Vacancy GetVacancy(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyGuid) ??
                          _vacancyPostingService.GetVacancy(viewModel.VacancyReferenceNumber);

            return vacancy;
        }

        public EmployerSearchViewModel GetProviderSiteEmployerLinkViewModels(string providerSiteErn)
        {
            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;
            var parameters = new EmployerSearchRequest(providerSiteErn);
            var providerSiteEmployerLinks = _providerService.GetVacancyParties(parameters, 1, pageSize);
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

            var providerSiteEmployerLinks = _providerService.GetVacancyParties(parameters, viewModel.EmployerResultsPage.CurrentPage, pageSize);

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