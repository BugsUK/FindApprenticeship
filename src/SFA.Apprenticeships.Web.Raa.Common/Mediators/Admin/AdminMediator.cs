﻿namespace SFA.Apprenticeships.Web.Raa.Common.Mediators.Admin
{
    using Application.Interfaces;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Constants.ViewModels;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Providers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants.Pages;
    using Validators.Api;
    using Validators.Provider;
    using Validators.ProviderUser;
    using ViewModels.Admin;
    using ViewModels.Api;
    using ViewModels.Provider;
    using ViewModels.ProviderUser;
    using Web.Common.Constants;
    using Web.Common.Mediators;

    public class AdminMediator : MediatorBase, IAdminMediator
    {
        private readonly ProviderSearchViewModelServerValidator _providerSearchViewModelServerValidator = new ProviderSearchViewModelServerValidator();
        private readonly ProviderViewModelServerValidator _providerViewModelServerValidator = new ProviderViewModelServerValidator();
        private readonly ProviderSiteSearchViewModelServerValidator _providerSiteSearchViewModelServerValidator = new ProviderSiteSearchViewModelServerValidator();
        private readonly ProviderSiteViewModelServerValidator _providerSiteViewModelServerValidator = new ProviderSiteViewModelServerValidator();
        private readonly ProviderSiteRelationshipViewModelServerValidator _providerSiteRelationshipViewModelServerValidator = new ProviderSiteRelationshipViewModelServerValidator();
        private readonly ApiUserSearchViewModelServerValidator _apiUserSearchViewModelServerValidator = new ApiUserSearchViewModelServerValidator();
        private readonly ApiUserViewModelServerValidator _apiUserViewModelServerValidator = new ApiUserViewModelServerValidator();
        private readonly ProviderUserSearchViewModelServerValidator _providerUserSearchViewModelServerValidator = new ProviderUserSearchViewModelServerValidator();

        private readonly IProviderProvider _providerProvider;
        private readonly IApiUserProvider _apiUserProvider;
        private readonly ILogService _logService;
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IProviderService _providerService;
        private readonly IVacancyPostingProvider _vacancyPostingProvider;
        private readonly IProviderUserProvider _providerUserProvider;

        public AdminMediator(IProviderProvider providerProvider, IApiUserProvider apiUserProvider, ILogService logService, IVacancyPostingService vacancyPostingService,
            IProviderService providerService, IVacancyPostingProvider vacancyPostingProvider, IProviderUserProvider providerUserProvider)
        {
            _providerProvider = providerProvider;
            _apiUserProvider = apiUserProvider;
            _logService = logService;
            _vacancyPostingService = vacancyPostingService;
            _providerService = providerService;
            _vacancyPostingProvider = vacancyPostingProvider;
            _providerUserProvider = providerUserProvider;
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

            var searchResultsViewModel = _apiUserProvider.SearchApiUsers(new ApiUserSearchViewModel { Id = viewModel.CompanyId, PerformSearch = true });
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

        public MediatorResponse<ApiUserViewModel> ResetApiUserPassword(ApiUserViewModel viewModel)
        {
            try
            {
                viewModel = _apiUserProvider.ResetApiUserPassword(viewModel);

                return GetMediatorResponse(AdminMediatorCodes.ResetApiUserPassword.Ok, viewModel, ApiUserViewModelMessages.ResetApiUserPasswordSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to save api user with external system id={viewModel.ExternalSystemId}", ex);
                viewModel = _apiUserProvider.GetApiUserViewModel(viewModel.ExternalSystemId);
                return GetMediatorResponse(AdminMediatorCodes.ResetApiUserPassword.Error, viewModel, ApiUserViewModelMessages.ResetApiUserPasswordError, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<TransferVacanciesResultsViewModel> GetVacancyDetails(TransferVacanciesViewModel viewModel)
        {
            try
            {
                var vacanciesToBeTransferred = new List<TransferVacancyViewModel>();
                var transferVacanciesResultsViewModel = new TransferVacanciesResultsViewModel
                {
                    TransferVacanciesViewModel = viewModel,
                    NotFoundVacancyNumbers = new List<string>()
                };

                if (viewModel != null && viewModel.VacancyReferenceNumbers.Any())
                {
                    foreach (var vacancy in viewModel.VacancyReferenceNumbers.Split(','))
                    {
                        string vacancyReference;
                        if (VacancyHelper.TryGetVacancyReference(vacancy, out vacancyReference))
                        {
                            var vacancyDetails = _vacancyPostingService.GetVacancyByReferenceNumber(Convert.ToInt32(vacancyReference));
                            if (vacancyDetails != null)
                            {
                                var vacancyView = new TransferVacancyViewModel
                                {
                                    ContractOwnerId = vacancyDetails.ContractOwnerId,
                                    VacancyManagerId = vacancyDetails.VacancyManagerId,
                                    VacancyReferenceNumber = vacancyDetails.VacancyReferenceNumber,
                                    DeliveryOrganisationId = vacancyDetails.DeliveryOrganisationId,
                                    VacancyOwnerRelationShipId = vacancyDetails.VacancyOwnerRelationshipId,
                                    ProviderName = _providerService.GetProvider(vacancyDetails.ContractOwnerId).TradingName,
                                    VacancyTitle = vacancyDetails.Title,
                                    EmployerName = vacancyDetails.EmployerName
                                };
                                if (vacancyDetails.VacancyManagerId.HasValue)
                                {
                                    vacancyView.ProviderSiteName =
                                        _providerService.GetProviderSite(vacancyDetails.VacancyManagerId.Value).TradingName;
                                }
                                vacanciesToBeTransferred.Add(vacancyView);
                            }
                            else
                                transferVacanciesResultsViewModel.NotFoundVacancyNumbers.Add(vacancy);
                        }
                    }
                }
                transferVacanciesResultsViewModel.VacanciesToBeTransferredVm = vacanciesToBeTransferred;
                return GetMediatorResponse(AdminMediatorCodes.GetVacancyDetails.Ok, transferVacanciesResultsViewModel);
            }
            catch (CustomException exception) when (exception.Code == ErrorCodes.ProviderVacancyAuthorisation.Failed)
            {
                return GetMediatorResponse(AdminMediatorCodes.GetVacancyDetails.FailedAuthorisation, new TransferVacanciesResultsViewModel(), TransferVacanciesMessages.UnAuthorisedAccess, UserMessageLevel.Warning);
            }
        }

        public MediatorResponse<ManageVacancyTransferResultsViewModel> ManageVacanciesTransfers(ManageVacancyTransferViewModel vacancyTransferViewModel)
        {
            try
            {
                var resultsViewModel = new ManageVacancyTransferResultsViewModel();
                if (vacancyTransferViewModel.ProviderId != 0 && vacancyTransferViewModel.ProviderSiteId != 0 && vacancyTransferViewModel.VacancyReferenceNumbers.Any())
                {
                    _vacancyPostingProvider.TransferVacancies(vacancyTransferViewModel);
                }
                return GetMediatorResponse(AdminMediatorCodes.TransferVacancy.Ok, resultsViewModel, TransferVacanciesMessages.Ok, UserMessageLevel.Success);
            }
            catch (Exception exception)
            {
                _logService.Error("Failed to transfer vacancies with vacancy referencenumbers" +
                                  $" ={string.Join(",", vacancyTransferViewModel.VacancyReferenceNumbers)}", exception);
                return GetMediatorResponse(AdminMediatorCodes.TransferVacancy.FailedTransfer, new ManageVacancyTransferResultsViewModel(), TransferVacanciesMessages.FailedTransfer, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ProviderUserSearchResultsViewModel> SearchProviderUsers(ProviderUserSearchViewModel searchViewModel, string ukprn)
        {
            ProviderUserSearchResultsViewModel viewModel;

            if (searchViewModel.PerformSearch)
            {
                var validatonResult = _providerUserSearchViewModelServerValidator.Validate(searchViewModel);

                if (!validatonResult.IsValid)
                {
                    return GetMediatorResponse(AdminMediatorCodes.SearchProviderUsers.FailedValidation, GetProviderUsers(ukprn), validatonResult);
                }

                viewModel = _providerUserProvider.SearchProviderUsers(searchViewModel);
            }
            else
            {
                viewModel = GetProviderUsers(ukprn);
            }

            return GetMediatorResponse(AdminMediatorCodes.SearchProviderUsers.Ok, viewModel);
        }

        public MediatorResponse<ProviderUserViewModel> GetProviderUser(int providerUserId)
        {
            var viewModel = _providerUserProvider.GetProviderUserViewModel(providerUserId);

            return GetMediatorResponse(AdminMediatorCodes.GetProviderUser.Ok, viewModel);
        }

        public MediatorResponse<ProviderUserViewModel> SaveProviderUser(ProviderUserViewModel viewModel)
        {
            try
            {
                viewModel = _providerUserProvider.SaveProviderUser(viewModel);

                return GetMediatorResponse(AdminMediatorCodes.SaveProviderUser.Ok, viewModel, ProviderUserViewModelMessages.ProviderUserSavedSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to save provider user with id={viewModel.ProviderUserId}", ex);
                viewModel = _providerUserProvider.GetProviderUserViewModel(viewModel.ProviderUserId);
                return GetMediatorResponse(AdminMediatorCodes.SaveProviderUser.Error, viewModel, ProviderUserViewModelMessages.ProviderUserSaveError, UserMessageLevel.Error);
            }
        }

        public MediatorResponse<ProviderUserViewModel> VerifyProviderUserEmail(ProviderUserViewModel viewModel)
        {
            try
            {
                viewModel = _providerUserProvider.VerifyProviderUserEmail(viewModel);

                return GetMediatorResponse(AdminMediatorCodes.VerifyProviderUserEmail.Ok, viewModel, ProviderUserViewModelMessages.VerifiedProviderUserEmailSuccessfully, UserMessageLevel.Info);
            }
            catch (Exception ex)
            {
                _logService.Error($"Failed to verify provider user's email with id={viewModel.ProviderUserId}", ex);
                viewModel = _providerUserProvider.GetProviderUserViewModel(viewModel.ProviderUserId);
                return GetMediatorResponse(AdminMediatorCodes.VerifyProviderUserEmail.Error, viewModel, ProviderUserViewModelMessages.VerifyProviderUserEmailError, UserMessageLevel.Error);
            }
        }

        private ProviderUserSearchResultsViewModel GetProviderUsers(string ukprn)
        {
            var provider = _providerService.GetProvider(ukprn);

            var viewModel = new ProviderUserSearchResultsViewModel
            {
                SearchViewModel = new ProviderUserSearchViewModel(),
                ProviderName = $"{provider.TradingName} ({provider.Ukprn})",
                ProviderUsers = _providerUserProvider.GetProviderUsers(ukprn).ToList()
            };

            return viewModel;
        }
    }
}