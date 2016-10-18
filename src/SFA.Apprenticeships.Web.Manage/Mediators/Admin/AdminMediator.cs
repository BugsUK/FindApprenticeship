namespace SFA.Apprenticeships.Web.Manage.Mediators.Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Linq;
    using System.Text;
    using Application.Interfaces;
    using Application.Interfaces.ReferenceData;
    using Application.ReferenceData;
    using Common.Constants;
    using Common.Mediators;
    using CsvClassMaps;
    using CsvHelper.Configuration;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Raa.Interfaces.Reporting.Models;
    using Domain.Raa.Interfaces.Repositories;
    using Glimpse.Core.Extensions;
    using Infrastructure.Presentation;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Api;
    using Raa.Common.Validators.Provider;
    using Raa.Common.ViewModels.Api;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Reporting;
    using ViewModels;
    using ViewModels.Admin;

    public class AdminMediator : MediatorBase, IAdminMediator
    {
        private readonly ProviderSearchViewModelServerValidator _providerSearchViewModelServerValidator = new ProviderSearchViewModelServerValidator();
        private readonly ProviderViewModelServerValidator _providerViewModelServerValidator = new ProviderViewModelServerValidator();
        private readonly ProviderSiteSearchViewModelServerValidator _providerSiteSearchViewModelServerValidator = new ProviderSiteSearchViewModelServerValidator();
        private readonly ProviderSiteViewModelServerValidator _providerSiteViewModelServerValidator = new ProviderSiteViewModelServerValidator();
        private readonly ProviderSiteRelationshipViewModelServerValidator _providerSiteRelationshipViewModelServerValidator = new ProviderSiteRelationshipViewModelServerValidator();
        private readonly ApiUserSearchViewModelServerValidator _apiUserSearchViewModelServerValidator = new ApiUserSearchViewModelServerValidator();
        private readonly ApiUserViewModelServerValidator _apiUserViewModelServerValidator = new ApiUserViewModelServerValidator();

        private readonly IProviderProvider _providerProvider;
        private readonly IApiUserProvider _apiUserProvider;
        private readonly ILogService _logService;
        private readonly IReferenceDataProvider _referenceDataProvider;

        public AdminMediator(IProviderProvider providerProvider, IApiUserProvider apiUserProvider, ILogService logService, IReferenceDataProvider referenceDataProvider)
        {
            _providerProvider = providerProvider;
            _apiUserProvider = apiUserProvider;
            _logService = logService;
            _referenceDataProvider = referenceDataProvider;
        }

        public MediatorResponse<ProviderSearchResultsViewModel> SearchProviders(ProviderSearchViewModel searchViewModel)
        {
            var validatonResult = _providerSearchViewModelServerValidator.Validate(searchViewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.SearchProviders.FailedValidation, new ProviderSearchResultsViewModel { SearchViewModel = searchViewModel }, validatonResult);
            }

            var viewModel = _providerProvider.SearchProviders(searchViewModel);

            return GetMediatorResponse(AdminMediatorCodes.SearchProviders.Ok, viewModel);
        }

        public MediatorResponse<ProviderViewModel> GetProvider(int providerId)
        {
            var viewModel = _providerProvider.GetProviderViewModel(providerId);

            return GetMediatorResponse(AdminMediatorCodes.GetProvider.Ok, viewModel);
        }

        public MediatorResponse<ProviderViewModel> CreateProvider(ProviderViewModel viewModel)
        {
            var validatonResult = _providerViewModelServerValidator.Validate(viewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProvider.FailedValidation, viewModel, validatonResult);
            }

            var existingViewModel = _providerProvider.GetProviderViewModel(viewModel.Ukprn, false);
            if (existingViewModel != null)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProvider.UkprnAlreadyExists, viewModel, ProviderViewModelMessages.UkprnAlreadyExists, UserMessageLevel.Error);
            }

            viewModel = _providerProvider.CreateProvider(viewModel);

            return GetMediatorResponse(AdminMediatorCodes.CreateProvider.Ok, viewModel, ProviderViewModelMessages.ProviderCreatedSuccessfully, UserMessageLevel.Info);
        }

        public MediatorResponse<ProviderViewModel> SaveProvider(ProviderViewModel viewModel)
        {
            try
            {
                viewModel = _providerProvider.SaveProvider(viewModel);

                return GetMediatorResponse(AdminMediatorCodes.SaveProvider.Ok, viewModel, ProviderViewModelMessages.ProviderSavedSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to save provider with id={viewModel.ProviderId}", ex);
                viewModel = _providerProvider.GetProviderViewModel(viewModel.ProviderId);
                return GetMediatorResponse(AdminMediatorCodes.SaveProvider.Error, viewModel, ProviderViewModelMessages.ProviderSaveError, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ProviderSiteSearchResultsViewModel> SearchProviderSites(ProviderSiteSearchViewModel searchViewModel)
        {
            var validatonResult = _providerSiteSearchViewModelServerValidator.Validate(searchViewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.SearchProviderSites.FailedValidation, new ProviderSiteSearchResultsViewModel { SearchViewModel = searchViewModel }, validatonResult);
            }

            var viewModel = _providerProvider.SearchProviderSites(searchViewModel);

            return GetMediatorResponse(AdminMediatorCodes.SearchProviderSites.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteViewModel> GetProviderSite(int providerSiteId)
        {
            var viewModel = _providerProvider.GetProviderSiteViewModel(providerSiteId);

            return GetMediatorResponse(AdminMediatorCodes.GetProviderSite.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteViewModel> CreateProviderSite(ProviderSiteViewModel viewModel)
        {
            var validatonResult = _providerSiteViewModelServerValidator.Validate(viewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProviderSite.FailedValidation, viewModel, validatonResult);
            }

            var existingViewModel = _providerProvider.GetProviderSiteViewModel(viewModel.EdsUrn);
            if (existingViewModel != null)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProviderSite.EdsUrnAlreadyExists, viewModel, ProviderSiteViewModelMessages.EdsUrnAlreadyExists, UserMessageLevel.Error);
            }

            viewModel = _providerProvider.CreateProviderSite(viewModel);

            return GetMediatorResponse(AdminMediatorCodes.CreateProviderSite.Ok, viewModel, ProviderSiteViewModelMessages.ProviderSiteCreatedSuccessfully, UserMessageLevel.Info);
        }

        public MediatorResponse<ProviderSiteViewModel> SaveProviderSite(ProviderSiteViewModel viewModel)
        {
            try
            {
                viewModel = _providerProvider.SaveProviderSite(viewModel);

                return GetMediatorResponse(AdminMediatorCodes.SaveProviderSite.Ok, viewModel, ProviderSiteViewModelMessages.ProviderSiteSavedSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to save provider site with id={viewModel.ProviderSiteId}", ex);
                viewModel = _providerProvider.GetProviderSiteViewModel(viewModel.ProviderSiteId);
                return GetMediatorResponse(AdminMediatorCodes.SaveProviderSite.Error, viewModel, ProviderSiteViewModelMessages.ProviderSiteSaveError, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ProviderSiteViewModel> CreateProviderSiteRelationship(ProviderSiteViewModel viewModel)
        {
            var existingViewModel = _providerProvider.GetProviderSiteViewModel(viewModel.ProviderSiteId);
            existingViewModel.ProviderUkprn = viewModel.ProviderUkprn;
            existingViewModel.ProviderSiteRelationshipType = viewModel.ProviderSiteRelationshipType;
            viewModel = existingViewModel;

            var validatonResult = _providerSiteRelationshipViewModelServerValidator.Validate(viewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProviderSiteRelationship.FailedValidation, viewModel, validatonResult);
            }

            var providerViewModel = _providerProvider.GetProviderViewModel(viewModel.ProviderUkprn);
            if (providerViewModel == null)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateProviderSiteRelationship.InvalidUkprn, viewModel, ProviderSiteViewModelMessages.ProviderSiteRelationshipInvalidUkprn, UserMessageLevel.Error);
            }

            try
            {
                viewModel = _providerProvider.CreateProviderSiteRelationship(viewModel, providerViewModel.ProviderId);

                return GetMediatorResponse(AdminMediatorCodes.CreateProviderSiteRelationship.Ok, viewModel, ProviderSiteViewModelMessages.ProviderSiteRelationshipCreatedSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to create provider site relationship for provider site with id={viewModel.ProviderSiteId}", ex);
                return GetMediatorResponse(AdminMediatorCodes.CreateProviderSiteRelationship.Error, viewModel, ProviderSiteViewModelMessages.ProviderSiteRelationshipCreationError, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ApiUserSearchResultsViewModel> SearchApiUsers(ApiUserSearchViewModel searchViewModel)
        {
            var validatonResult = _apiUserSearchViewModelServerValidator.Validate(searchViewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.SearchApiUsers.FailedValidation, new ApiUserSearchResultsViewModel { SearchViewModel = searchViewModel }, validatonResult);
            }

            var viewModel = _apiUserProvider.SearchApiUsers(searchViewModel);

            return GetMediatorResponse(AdminMediatorCodes.SearchApiUsers.Ok, viewModel);
        }

        public MediatorResponse<ApiUserViewModel> GetApiUser(Guid externalSystemId)
        {
            var viewModel = _apiUserProvider.GetApiUserViewModel(externalSystemId);

            return GetMediatorResponse(AdminMediatorCodes.GetApiUser.Ok, viewModel);
        }

        public MediatorResponse<ApiUserViewModel> CreateApiUser(ApiUserViewModel viewModel)
        {
            var validatonResult = _apiUserViewModelServerValidator.Validate(viewModel);

            if (!validatonResult.IsValid)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateApiUser.FailedValidation, viewModel, validatonResult);
            }

            var searchResultsViewModel = _apiUserProvider.SearchApiUsers(new ApiUserSearchViewModel {Id = viewModel.CompanyId, PerformSearch = true});
            if (searchResultsViewModel.ApiUsers.Count > 0)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateApiUser.CompanyIdAlreadyExists, viewModel, ApiUserViewModelMessages.CompanyIdAlreadyExists, UserMessageLevel.Error);
            }

            var viewModelToCreate = _apiUserProvider.GetApiUserViewModel(viewModel.CompanyId);
            if (viewModelToCreate == null)
            {
                return GetMediatorResponse(AdminMediatorCodes.CreateApiUser.UnknownCompanyId, viewModel, ApiUserViewModelMessages.UnknownCompanyId, UserMessageLevel.Error);
            }

            viewModelToCreate.ExternalSystemId = viewModel.ExternalSystemId;
            viewModelToCreate.Password = viewModel.Password;
            viewModelToCreate.ApiEndpoints = viewModel.ApiEndpoints;

            try
            {
                viewModel = _apiUserProvider.CreateApiUser(viewModelToCreate);

                return GetMediatorResponse(AdminMediatorCodes.CreateApiUser.Ok, viewModel, ApiUserViewModelMessages.ApiUserCreatedSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to create API user for company with id={viewModel.CompanyId}", ex);
                return GetMediatorResponse(AdminMediatorCodes.CreateApiUser.Error, viewModel, ApiUserViewModelMessages.ApiUserCreationError, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ApiUserViewModel> SaveApiUser(ApiUserViewModel viewModel)
        {
            try
            {
                viewModel = _apiUserProvider.SaveApiUser(viewModel);

                return GetMediatorResponse(AdminMediatorCodes.SaveApiUser.Ok, viewModel, ApiUserViewModelMessages.ApiUserSavedSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to save api user with external system id={viewModel.ExternalSystemId}", ex);
                viewModel = _apiUserProvider.GetApiUserViewModel(viewModel.ExternalSystemId);
                return GetMediatorResponse(AdminMediatorCodes.SaveApiUser.Error, viewModel, ApiUserViewModelMessages.ApiUserSaveError, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<List<StandardSubjectAreaTierOne>> GetStandard()
        {
            var viewModel = _referenceDataProvider.GetStandardSubjectAreaTierOnes().ToList();

            return GetMediatorResponse(AdminMediatorCodes.GetStandard.Ok, viewModel);
        }

        public MediatorResponse<List<Category>> GetFrameworks()
        {
            var viewModel = _referenceDataProvider.GetFrameworks().ToList();
            return GetMediatorResponse(AdminMediatorCodes.GetFrameworks.Ok, viewModel);
        }

        public MediatorResponse<byte[]> GetFrameworksBytes()
        {
            try
            {
                var result = _referenceDataProvider.GetFrameworks().ToList();

                var frameworkResult =
                    result.SelectMany(
                        r =>
                            r.SubCategories.Select(
                                s =>
                                    new FrameworkViewModel()
                                    {
                                        SSAT1Name = r.FullName,
                                        FrameworkStatus = s.Status,
                                        FrameworkId = s.Id,
                                        FrameworkFullName = s.FullName
                                    }));

                //Convert to list ofFrameworkData
                //Pass that list into function
                var bytes = GetCsvBytes<FrameworkViewModel, FrameworkDataClassMap>(frameworkResult, "");
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, new byte[0]);
            }
        }

        public MediatorResponse<byte[]> GetStandardsBytes()
        {
            try
            {
                var result = _referenceDataProvider.GetStandardSubjectAreaTierOnes();

                var standardSubjectAreaTierOneResult =
                    result.SelectMany(
                        r =>
                            r.Sectors.SelectMany(
                                ss =>
                                    ss.Standards.Select(
                                        s =>
                                            new StandardSubjectAreaTierOneViewModel()
                                            {
                                                SSAT1Name = r.Name,
                                                StandardId = s.Id,
                                                StandardSectorName = ss.Name,
                                                StandardName = s.Name
                                            })));

                var bytes = GetCsvBytes<StandardSubjectAreaTierOneViewModel, StandardSubjectAreaTierOneClassMap>(standardSubjectAreaTierOneResult, "");
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, new byte[0]);
            }
        }

        private static byte[] GetCsvBytes<T, TClassMap>(IEnumerable<T> items, string header) where T : class where TClassMap : CsvClassMap<T>
        {
            var csvString = header + CsvPresenter.ToCsv<T, TClassMap>(items);
            var bytes = Encoding.UTF8.GetBytes(csvString);
            return bytes;
        }
    }
}