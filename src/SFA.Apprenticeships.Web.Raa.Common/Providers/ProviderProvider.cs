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
    using Domain.Entities.Raa.Vacancies;
    using Application.Interfaces;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Mappers;
    using ViewModels.Provider;
    using ViewModels.VacancyPosting;
    using Web.Common.Converters;

    public class ProviderProvider : IProviderProvider, IProviderQAProvider
    {
        private readonly IMapper _providerMappers = new ProviderMappers();

        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IProviderService _providerService;
        private readonly IEmployerService _employerService;
        private readonly IConfigurationService _configurationService;

        public ProviderProvider(IProviderService providerService, IConfigurationService configurationService, IVacancyPostingService vacancyPostingService, IEmployerService employerService)
        {
            _providerService = providerService;
            _configurationService = configurationService;
            _vacancyPostingService = vacancyPostingService;
            _employerService = employerService;
        }

        public ProviderViewModel GetProviderViewModel(string ukprn, bool errorIfNotFound = true)
        {
            var provider = _providerService.GetProvider(ukprn, errorIfNotFound);
            if (provider == null)
            {
                return null;
            }

            var providerSites = _providerService.GetProviderSites(ukprn);

            return provider.Convert(providerSites);
        }

        public ProviderViewModel GetProviderViewModel(int providerId)
        {
            var provider = _providerService.GetProvider(providerId);
            var providerSites = _providerService.GetProviderSites(provider.Ukprn);

            return provider.Convert(providerSites);
        }

        public ProviderSearchResultsViewModel SearchProviders(ProviderSearchViewModel searchViewModel)
        {
            var viewModel = new ProviderSearchResultsViewModel
            {
                SearchViewModel = searchViewModel
            };

            if(!searchViewModel.PerformSearch) return viewModel;

            var searchParameters = new ProviderSearchParameters
            {
                Ukprn = searchViewModel.Ukprn,
                Name = searchViewModel.Name
            };

            var providers = _providerService.SearchProviders(searchParameters);

            viewModel.Providers = providers.Select(p => p.Convert()).ToList();

            return viewModel;
        }

        public ProviderSiteViewModel GetProviderSiteViewModel(string edsUrn)
        {
            var providerSite = _providerService.GetProviderSite(edsUrn);

            return providerSite?.Convert();
        }

        public IEnumerable<ProviderSiteViewModel> GetProviderSiteViewModels(string ukprn)
        {
            var providerSites = _providerService.GetProviderSites(ukprn);
            return providerSites.Select(ps => ps.Convert());
        }

        public VacancyPartyViewModel GetVacancyPartyViewModel(int vacancyPartyId)
        {
            var vacancyParty = _providerService.GetVacancyParty(vacancyPartyId, true);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);
            return vacancyParty.Convert(employer);
        }

        public VacancyPartyViewModel GetVacancyPartyViewModel(int providerSiteId, string edsUrn)
        {
            var vacancyParty = _providerService.GetVacancyParty(providerSiteId, edsUrn);
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);
            return vacancyParty.Convert(employer);
        }

        public VacancyPartyViewModel ConfirmVacancyParty(VacancyPartyViewModel viewModel)
        {
            if (_providerService.IsADeletedVacancyParty(viewModel.ProviderSiteId, viewModel.Employer.EdsUrn))
            {
                _providerService.ResurrectVacancyParty(viewModel.ProviderSiteId, viewModel.Employer.EdsUrn);
            } 

            var vacancyParty = _providerService.GetVacancyParty(viewModel.ProviderSiteId, viewModel.Employer.EdsUrn);
            vacancyParty.EmployerWebsiteUrl = viewModel.EmployerWebsiteUrl;
            vacancyParty.EmployerDescription = viewModel.EmployerDescription;
            vacancyParty = _providerService.SaveVacancyParty(vacancyParty);

            var vacancy = GetVacancy(viewModel);
            if (vacancy != null)
            {
                vacancy.OwnerPartyId = vacancyParty.VacancyPartyId;
                vacancy.EmployerWebsiteUrl = vacancyParty.EmployerWebsiteUrl;
                vacancy.EmployerDescription = vacancyParty.EmployerDescription;
                if (viewModel.IsEmployerLocationMainApprenticeshipLocation != null)
                    vacancy.IsEmployerLocationMainApprenticeshipLocation =
                        viewModel.IsEmployerLocationMainApprenticeshipLocation.Value;
                if (viewModel.NumberOfPositions != null) vacancy.NumberOfPositions = viewModel.NumberOfPositions.Value;

                _vacancyPostingService.UpdateVacancy(vacancy);
            }

            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, true);
            var result = vacancyParty.Convert(employer);
            
            return result;
        }

        private Vacancy GetVacancy(VacancyPartyViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyGuid) ??
                          _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            return vacancy;
        }

        public EmployerSearchViewModel GetVacancyPartyViewModels(int providerSiteId)
        {
            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;
            var parameters = new EmployerSearchRequest(providerSiteId);
            var vacancyParties = _providerService.GetVacancyParties(parameters, 1, pageSize);

            var employerIds = vacancyParties.Page
                .Select(vacancyParty => vacancyParty.EmployerId)
                .Distinct();

            var employers = _employerService.GetEmployers(employerIds);

            var resultsPage = vacancyParties.ToViewModel(vacancyParties.Page
                // Exclude employers from search results that are NOT returned from Employer Service, status may be 'Suspended' etc.
                .Where(vacancyParty => employers
                    .Any(employer => employer.EmployerId == vacancyParty.EmployerId))
                .Select(vacancyParty => vacancyParty.Convert(employers.Single(employer => employer.EmployerId == vacancyParty.EmployerId))
                    .Employer
                    .ConvertToResult()));

            return new EmployerSearchViewModel
            {
                ProviderSiteId = providerSiteId,
                EmployerResultsPage = resultsPage
            };
        }

        public EmployerSearchViewModel GetVacancyPartyViewModels(EmployerSearchViewModel viewModel)
        {
            EmployerSearchRequest parameters;

            switch (viewModel.FilterType)
            {
                case EmployerFilterType.EdsUrn:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteId, viewModel.EdsUrn);
                    break;
                case EmployerFilterType.NameAndLocation:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteId, viewModel.Name, viewModel.Location);
                    break;
                case EmployerFilterType.Undefined:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewModel), viewModel.FilterType, string.Empty);
            }

            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;
            var vacancyParties = _providerService.GetVacancyParties(parameters, viewModel.EmployerResultsPage.CurrentPage, pageSize);

            var employerIds = vacancyParties.Page
                .Select(vp => vp.EmployerId)
                .Distinct();

            var employers = _employerService.GetEmployers(employerIds);

            var resultsPage = vacancyParties.ToViewModel(vacancyParties.Page
                // Exclude employers from search results that are NOT returned from Employer Service, status may be 'Suspended' etc.
                .Where(vacancyParty => employers
                    .Any(employer => employer.EmployerId == vacancyParty.EmployerId))
                .Select(vacancyParty => vacancyParty.Convert(employers.Single(employer => employer.EmployerId == vacancyParty.EmployerId))
                    .Employer
                    .ConvertToResult()));

            viewModel.EmployerResultsPage = resultsPage;

            return viewModel;
        }

        public ProviderViewModel CreateProvider(ProviderViewModel viewModel)
        {
            var provider = _providerMappers.Map<ProviderViewModel, Provider>(viewModel);
            provider.IsMigrated = true;

            provider = _providerService.CreateProvider(provider);

            return _providerMappers.Map<Provider, ProviderViewModel>(provider);
        }

        public ProviderSiteViewModel CreateProviderSite(ProviderSiteViewModel viewModel)
        {
            var providerSite = _providerMappers.Map<ProviderSiteViewModel, ProviderSite>(viewModel);
            //Create a relationship between the provider and new provider site
            providerSite.ProviderSiteRelationships = new List<ProviderSiteRelationship>
            {
                new ProviderSiteRelationship
                {
                    ProviderId = viewModel.ProviderId,
                    ProviderSiteRelationShipTypeId = ProviderSiteRelationshipTypes.Owner
                }
            };

            providerSite = _providerService.CreateProviderSite(providerSite);

            var providerSiteViewModel = _providerMappers.Map<ProviderSite, ProviderSiteViewModel>(providerSite);
            providerSiteViewModel.ProviderId = viewModel.ProviderId;

            return providerSiteViewModel;
        }
    }
}
