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
    using ViewModels.Employer;
    using ViewModels.Provider;
    using Web.Common.Converters;

    public class ProviderProvider : IProviderProvider, IProviderQAProvider
    {
        private static readonly IMapper ProviderMappers = new ProviderMappers();

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
            var providerSites = _providerService.GetProviderSites(providerId);

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
                Id = searchViewModel.Id,
                Ukprn = searchViewModel.Ukprn,
                Name = searchViewModel.Name
            };

            var providers = _providerService.SearchProviders(searchParameters);

            viewModel.Providers = providers.Select(p => p.Convert()).ToList();

            return viewModel;
        }

        public ProviderSiteViewModel GetProviderSiteViewModel(int providerSiteId)
        {
            var providerSite = _providerService.GetProviderSite(providerSiteId);

            return providerSite?.Convert();
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

        public ProviderSiteSearchResultsViewModel SearchProviderSites(ProviderSiteSearchViewModel searchViewModel)
        {
            var viewModel = new ProviderSiteSearchResultsViewModel
            {
                SearchViewModel = searchViewModel
            };

            if (!searchViewModel.PerformSearch) return viewModel;

            var searchParameters = new ProviderSiteSearchParameters
            {
                Id = searchViewModel.Id,
                EdsUrn = searchViewModel.EdsUrn,
                Name = searchViewModel.Name
            };

            var providerSites = _providerService.SearchProviderSites(searchParameters);

            viewModel.ProviderSites = providerSites.Select(p => p.Convert()).ToList();

            return viewModel;
        }

        public VacancyOwnerRelationshipViewModel GetVacancyOwnerRelationshipViewModel(int vacancyOwnerRelationshipId)
        {
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(vacancyOwnerRelationshipId, true);
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, true);
            return vacancyOwnerRelationship.Convert(employer);
        }

        public VacancyOwnerRelationshipViewModel GetVacancyOwnerRelationshipViewModel(int providerSiteId, string edsUrn)
        {
            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(providerSiteId, edsUrn);
            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, true);
            return vacancyOwnerRelationship.Convert(employer);
        }

        public VacancyOwnerRelationshipViewModel ConfirmVacancyOwnerRelationship(VacancyOwnerRelationshipViewModel viewModel)
        {
            if (_providerService.IsADeletedVacancyOwnerRelationship(viewModel.ProviderSiteId, viewModel.Employer.EdsUrn))
            {
                _providerService.ResurrectVacancyOwnerRelationship(viewModel.ProviderSiteId, viewModel.Employer.EdsUrn);
            } 

            var vacancyOwnerRelationship = _providerService.GetVacancyOwnerRelationship(viewModel.ProviderSiteId, viewModel.Employer.EdsUrn);
            vacancyOwnerRelationship.EmployerWebsiteUrl = viewModel.EmployerWebsiteUrl;
            vacancyOwnerRelationship.EmployerDescription = viewModel.EmployerDescription;
            vacancyOwnerRelationship = _providerService.SaveVacancyOwnerRelationship(vacancyOwnerRelationship);

            var vacancy = GetVacancy(viewModel);
            if (vacancy != null)
            {
                vacancy.VacancyOwnerRelationshipId = vacancyOwnerRelationship.VacancyOwnerRelationshipId;
                vacancy.EmployerWebsiteUrl = vacancyOwnerRelationship.EmployerWebsiteUrl;
                vacancy.EmployerDescription = vacancyOwnerRelationship.EmployerDescription;
                if (viewModel.IsEmployerLocationMainApprenticeshipLocation != null)
                    vacancy.IsEmployerLocationMainApprenticeshipLocation =
                        viewModel.IsEmployerLocationMainApprenticeshipLocation.Value;
                if (viewModel.NumberOfPositions != null) vacancy.NumberOfPositions = viewModel.NumberOfPositions.Value;

                _vacancyPostingService.UpdateVacancy(vacancy);
            }

            var employer = _employerService.GetEmployer(vacancyOwnerRelationship.EmployerId, true);
            var result = vacancyOwnerRelationship.Convert(employer);
            
            return result;
        }

        private Vacancy GetVacancy(VacancyOwnerRelationshipViewModel viewModel)
        {
            var vacancy = _vacancyPostingService.GetVacancy(viewModel.VacancyGuid) ??
                          _vacancyPostingService.GetVacancyByReferenceNumber(viewModel.VacancyReferenceNumber);

            return vacancy;
        }

        public EmployerSearchViewModel GetVacancyOwnerRelationshipViewModels(int providerSiteId)
        {
            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;
            var parameters = new EmployerSearchRequest(providerSiteId);
            var vacancyParties = _providerService.GetVacancyOwnerRelationships(parameters, 1, pageSize);

            var employerIds = vacancyParties.Page
                .Select(vacancyOwnerRelationship => vacancyOwnerRelationship.EmployerId)
                .Distinct();

            var employers = _employerService.GetEmployers(employerIds);

            var resultsPage = vacancyParties.ToViewModel(vacancyParties.Page
                // Exclude employers from search results that are NOT returned from Employer Service, status may be 'Suspended' etc.
                .Where(vacancyOwnerRelationship => employers
                    .Any(employer => employer.EmployerId == vacancyOwnerRelationship.EmployerId))
                .Select(vacancyOwnerRelationship => vacancyOwnerRelationship.Convert(employers.Single(employer => employer.EmployerId == vacancyOwnerRelationship.EmployerId))
                    .Employer));

            return new EmployerSearchViewModel
            {
                ProviderSiteId = providerSiteId,
                Employers = resultsPage
            };
        }

        public EmployerSearchViewModel GetVacancyOwnerRelationshipViewModels(EmployerSearchViewModel viewModel)
        {
            EmployerSearchRequest parameters;

            switch (viewModel.FilterType)
            {
                case EmployerFilterType.EdsUrn:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteId, viewModel.EdsUrn);
                    break;
                case EmployerFilterType.NameAndLocation:
                case EmployerFilterType.NameOrLocation:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteId, viewModel.Name, viewModel.Location);
                    break;
                case EmployerFilterType.Undefined:
                    parameters = new EmployerSearchRequest(viewModel.ProviderSiteId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewModel), viewModel.FilterType, string.Empty);
            }

            var pageSize = _configurationService.Get<RecruitWebConfiguration>().PageSize;
            var vacancyParties = _providerService.GetVacancyOwnerRelationships(parameters, viewModel.Employers.CurrentPage, pageSize);

            var employerIds = vacancyParties.Page
                .Select(vp => vp.EmployerId)
                .Distinct();

            var employers = _employerService.GetEmployers(employerIds);

            var resultsPage = vacancyParties.ToViewModel(vacancyParties.Page
                // Exclude employers from search results that are NOT returned from Employer Service, status may be 'Suspended' etc.
                .Where(vacancyOwnerRelationship => employers
                    .Any(employer => employer.EmployerId == vacancyOwnerRelationship.EmployerId))
                .Select(vacancyOwnerRelationship => vacancyOwnerRelationship.Convert(employers.Single(employer => employer.EmployerId == vacancyOwnerRelationship.EmployerId))
                    .Employer));

            viewModel.Employers = resultsPage;

            return viewModel;
        }

        public ProviderViewModel CreateProvider(ProviderViewModel viewModel)
        {
            var provider = ProviderMappers.Map<ProviderViewModel, Provider>(viewModel);
            provider.IsMigrated = true;

            provider = _providerService.CreateProvider(provider);

            return ProviderMappers.Map<Provider, ProviderViewModel>(provider);
        }

        public ProviderViewModel SaveProvider(ProviderViewModel viewModel)
        {
            var provider = _providerService.GetProvider(viewModel.ProviderId);

            //Copy over changes
            provider.FullName = viewModel.FullName;
            provider.TradingName = viewModel.TradingName;
            provider.ProviderStatusType = viewModel.ProviderStatusType;

            var updatedProvider = _providerService.SaveProvider(provider);

            return ProviderMappers.Map<Provider, ProviderViewModel>(updatedProvider);
        }

        public ProviderSiteViewModel CreateProviderSite(ProviderSiteViewModel viewModel)
        {
            var providerSite = ProviderMappers.Map<ProviderSiteViewModel, ProviderSite>(viewModel);
            //Create a relationship between the provider and new provider site
            providerSite.ProviderSiteRelationships = new List<ProviderSiteRelationship>
            {
                new ProviderSiteRelationship
                {
                    ProviderId = viewModel.ProviderId,
                    ProviderSiteRelationShipTypeId = ProviderSiteRelationshipTypes.Owner
                }
            };
            providerSite.TrainingProviderStatus = EmployerTrainingProviderStatuses.Activated;

            providerSite = _providerService.CreateProviderSite(providerSite);

            var providerSiteViewModel = ProviderMappers.Map<ProviderSite, ProviderSiteViewModel>(providerSite);
            providerSiteViewModel.ProviderId = viewModel.ProviderId;

            return providerSiteViewModel;
        }

        public ProviderSiteViewModel SaveProviderSite(ProviderSiteViewModel viewModel)
        {
            var providerSite = _providerService.GetProviderSite(viewModel.ProviderSiteId);

            //Copy over changes
            providerSite.FullName = viewModel.FullName;
            providerSite.TradingName = viewModel.TradingName;
            providerSite.EmployerDescription = viewModel.EmployerDescription;
            providerSite.CandidateDescription = viewModel.CandidateDescription;
            providerSite.ContactDetailsForEmployer = viewModel.ContactDetailsForEmployer;
            providerSite.ContactDetailsForCandidate = viewModel.ContactDetailsForCandidate;
            providerSite.TrainingProviderStatus = viewModel.TrainingProviderStatus;
            if(viewModel.ProviderSiteRelationships != null)
            {
                foreach (var providerSiteRelationshipViewModel in viewModel.ProviderSiteRelationships)
                {
                    var providerSiteRelationship = providerSite.ProviderSiteRelationships.SingleOrDefault(psr => psr.ProviderSiteRelationshipId == providerSiteRelationshipViewModel.ProviderSiteRelationshipId);
                    if (providerSiteRelationship != null)
                    {
                        providerSiteRelationship.ProviderSiteRelationShipTypeId = providerSiteRelationshipViewModel.ProviderSiteRelationshipType;
                    }
                }
            }

            var updatedProviderSite = _providerService.SaveProviderSite(providerSite);

            return ProviderMappers.Map<ProviderSite, ProviderSiteViewModel>(updatedProviderSite);
        }

        public ProviderSiteViewModel CreateProviderSiteRelationship(ProviderSiteViewModel viewModel, int providerId)
        {
            var providerSiteRelationship = new ProviderSiteRelationship
            {
                ProviderId = providerId,
                ProviderSiteId = viewModel.ProviderSiteId,
                ProviderSiteRelationShipTypeId = viewModel.ProviderSiteRelationshipType
            };

            _providerService.CreateProviderSiteRelationship(providerSiteRelationship);

            return GetProviderSiteViewModel(viewModel.ProviderSiteId);
        }

        public ProviderSiteRelationshipViewModel GetProviderSiteRelationshipViewModel(int providerSiteRelationshipId)
        {
            var providerSiteRelationship = _providerService.GetProviderSiteRelationship(providerSiteRelationshipId);

            return providerSiteRelationship.Convert();
        }

        public void DeleteProviderSiteRelationship(int providerSiteRelationshipId)
        {
            _providerService.DeleteProviderSiteRelationship(providerSiteRelationshipId);
        }
    }
}
