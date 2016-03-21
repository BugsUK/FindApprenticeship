﻿namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Constants;
    using Common.Mediators;
    using FluentValidation;
    using Common.Validators;
    using Common.ViewModels;
    using Constants.ViewModels;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Vacancies;
    using Raa.Common.Converters;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Provider;
    using Raa.Common.Validators.VacancyPosting;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.VacancyPosting;

    public class VacancyMediator : MediatorBase, IVacancyMediator
    {
        private readonly IVacancyQAProvider _vacancyQaProvider;
        private readonly IProviderQAProvider _providerQaProvider;
        private readonly ILocationsProvider _locationsProvider;

        private readonly VacancyViewModelValidator _vacancyViewModelValidator;
        private readonly VacancySummaryViewModelServerValidator _vacancySummaryViewModelServerValidator;
        private readonly NewVacancyViewModelServerValidator _newVacancyViewModelServerValidator;
        private readonly TrainingDetailsViewModelServerValidator _trainingDetailsViewModelServerValidator;
        private readonly VacancyQuestionsViewModelServerValidator _vacancyQuestionsViewModelServerValidator;
        private readonly VacancyRequirementsProspectsViewModelServerValidator _vacancyRequirementsProspectsViewModelServerValidator;
        private readonly VacancyPartyViewModelValidator _vacancyPartyViewModelValidator;
        private readonly LocationSearchViewModelServerValidator _locationSearchViewModelServerValidator;

        public VacancyMediator(IVacancyQAProvider vacancyQaProvider,
            VacancyViewModelValidator vacancyViewModelValidator,
            VacancySummaryViewModelServerValidator vacancySummaryViewModelServerValidator,
            NewVacancyViewModelServerValidator newVacancyViewModelServerValidator, 
            VacancyQuestionsViewModelServerValidator vacancyQuestionsViewModelServerValidator,
            VacancyRequirementsProspectsViewModelServerValidator vacancyRequirementsProspectsViewModelServerValidator, 
            VacancyPartyViewModelValidator vacancyPartyViewModelValidator, 
            IProviderQAProvider providerQaProvider, LocationSearchViewModelServerValidator locationSearchViewModelServerValidator, ILocationsProvider locationsProvider, TrainingDetailsViewModelServerValidator trainingDetailsViewModelServerValidator)
        {
            _vacancyQaProvider = vacancyQaProvider;
            _vacancyViewModelValidator = vacancyViewModelValidator;
            _vacancySummaryViewModelServerValidator = vacancySummaryViewModelServerValidator;
            _newVacancyViewModelServerValidator = newVacancyViewModelServerValidator;
            _vacancyQuestionsViewModelServerValidator = vacancyQuestionsViewModelServerValidator;
            _vacancyRequirementsProspectsViewModelServerValidator = vacancyRequirementsProspectsViewModelServerValidator;
            _vacancyPartyViewModelValidator = vacancyPartyViewModelValidator;
            _providerQaProvider = providerQaProvider;
            _locationSearchViewModelServerValidator = locationSearchViewModelServerValidator;
            _locationsProvider = locationsProvider;
            _trainingDetailsViewModelServerValidator = trainingDetailsViewModelServerValidator;
        }

        public MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(int vacancyReferenceNumber)
        {
            if (_vacancyQaProvider.ApproveVacancy(vacancyReferenceNumber) == QAActionResult.InvalidVacancy)
            {
                return
                    GetMediatorResponse<DashboardVacancySummaryViewModel>(
                        VacancyMediatorCodes.ApproveVacancy.InvalidVacancy, null,
                        VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
            }

            var nextVacancy = _vacancyQaProvider.GetNextAvailableVacancy();

            return nextVacancy == null
                ? GetMediatorResponse<DashboardVacancySummaryViewModel>(
                    VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies)
                : GetMediatorResponse(VacancyMediatorCodes.ApproveVacancy.Ok, nextVacancy);
        }

        public MediatorResponse<DashboardVacancySummaryViewModel> RejectVacancy(int vacancyReferenceNumber)
        {
            if (_vacancyQaProvider.RejectVacancy(vacancyReferenceNumber) == QAActionResult.InvalidVacancy)
            {
                return
                    GetMediatorResponse<DashboardVacancySummaryViewModel>(
                        VacancyMediatorCodes.RejectVacancy.InvalidVacancy, null,
                        VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
            }

            var nextVacancy = _vacancyQaProvider.GetNextAvailableVacancy();

            return nextVacancy == null
                ? GetMediatorResponse<DashboardVacancySummaryViewModel>(
                    VacancyMediatorCodes.RejectVacancy.NoAvailableVacancies)
                : GetMediatorResponse(VacancyMediatorCodes.RejectVacancy.Ok, nextVacancy);
        }

        public MediatorResponse<VacancyViewModel> ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.ReserveVacancyForQA(vacancyReferenceNumber);

            if (vacancyViewModel == null)
            {
                return GetMediatorResponse<VacancyViewModel>(VacancyMediatorCodes.ReserveVacancyForQA.NoVacanciesAvailable, null, VacancyViewModelMessages.NoVacanciesAvailble, UserMessageLevel.Info);
            }

            if (vacancyViewModel.VacancyReferenceNumber != vacancyReferenceNumber)
            {
                return
                    GetMediatorResponse(VacancyMediatorCodes.ReserveVacancyForQA.NextAvailableVacancy, vacancyViewModel,
                        VacancyViewModelMessages.NextAvailableVacancy, UserMessageLevel.Info);
            }

            return GetMediatorResponse(VacancyMediatorCodes.ReserveVacancyForQA.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyViewModel> ReviewVacancy(int vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.ReviewVacancy(vacancyReferenceNumber);

            if (vacancyViewModel == null)
            {
                return GetMediatorResponse<VacancyViewModel>(VacancyMediatorCodes.ReviewVacancy.InvalidVacancy, null,
                    VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
            }

            var validationResult = _vacancyViewModelValidator.Validate(vacancyViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.ReviewVacancy.FailedValidation,
                    vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.ReviewVacancy.Ok, vacancyViewModel);
        }

        public MediatorResponse<FurtherVacancyDetailsViewModel> GetVacancySummaryViewModel(int vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancySummaryViewModel(vacancyReferenceNumber);

            var validationResult = _vacancySummaryViewModelServerValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid )
            {
                vacancyViewModel.WageUnits = ApprenticeshipVacancyConverter.GetWageUnits();
                vacancyViewModel.DurationTypes = ApprenticeshipVacancyConverter.GetDurationTypes(vacancyViewModel.VacancyType);

                return GetMediatorResponse(VacancyMediatorCodes.GetVacancySummaryViewModel.FailedValidation, vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancySummaryViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<FurtherVacancyDetailsViewModel> UpdateVacancy(FurtherVacancyDetailsViewModel viewModel)
        {
            var validationResult = _vacancySummaryViewModelServerValidator.Validate(viewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid && (!viewModel.AcceptWarnings || validationResult.Errors.Any(e => (ValidationType?)e.CustomState != ValidationType.Warning)))
            {
                viewModel.WageUnits = ApprenticeshipVacancyConverter.GetWageUnits();
                viewModel.DurationTypes = ApprenticeshipVacancyConverter.GetDurationTypes(viewModel.VacancyType);

                viewModel.AcceptWarnings = true;

                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<NewVacancyViewModel> GetBasicDetails(int vacancyReferenceNumber)
        {
            var newVacancyViewModel = _vacancyQaProvider.GetNewVacancyViewModel(vacancyReferenceNumber);

            var validationResult = _newVacancyViewModelServerValidator.Validate(newVacancyViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetBasicVacancyDetails.FailedValidation,
                    newVacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetBasicVacancyDetails.Ok, newVacancyViewModel);
        }

        public MediatorResponse<TrainingDetailsViewModel> GetTrainingDetails(int vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetTrainingDetailsViewModel(vacancyReferenceNumber);

            var validationResult = _trainingDetailsViewModelServerValidator.Validate(vacancyViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetTrainingDetails.FailedValidation, vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetTrainingDetails.Ok, vacancyViewModel);
        }

        public MediatorResponse<TrainingDetailsViewModel> SelectFrameworkAsTrainingType(TrainingDetailsViewModel viewModel)
        {
            viewModel.TrainingType = TrainingType.Frameworks;
            viewModel.ApprenticeshipLevel = ApprenticeshipLevel.Unknown;
            viewModel.FrameworkCodeName = null;
            viewModel.SectorCodeName = null;
            viewModel.Standards = _vacancyQaProvider.GetStandards();
            viewModel.SectorsAndFrameworks = _vacancyQaProvider.GetSectorsAndFrameworks();
            viewModel.Sectors = _vacancyQaProvider.GetSectors();

            return GetMediatorResponse(VacancyMediatorCodes.SelectFrameworkAsTrainingType.Ok, viewModel);
        }

        public MediatorResponse<TrainingDetailsViewModel> SelectStandardAsTrainingType(TrainingDetailsViewModel viewModel)
        {
            viewModel.TrainingType = TrainingType.Standards;
            viewModel.StandardId = null;
            viewModel.SectorCodeName = null;
            viewModel.ApprenticeshipLevel = ApprenticeshipLevel.Unknown;
            viewModel.Standards = _vacancyQaProvider.GetStandards();
            viewModel.SectorsAndFrameworks = _vacancyQaProvider.GetSectorsAndFrameworks();
            viewModel.Sectors = _vacancyQaProvider.GetSectors();

            return GetMediatorResponse(VacancyMediatorCodes.SelectStandardAsTrainingType.Ok, viewModel);
        }

        public MediatorResponse<VacancyViewModel> GetVacancyViewModel(int vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancy(vacancyReferenceNumber);
            vacancyViewModel.IsEditable = false;
            return GetMediatorResponse(VacancyMediatorCodes.GetVacancyViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(int vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancyQuestionsViewModel(vacancyReferenceNumber);

            var validationResult = _vacancyQuestionsViewModelServerValidator.Validate(vacancyViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetVacancyQuestionsViewModel.FailedValidation, vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancyQuestionsViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(int vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber);

            var validationResult = _vacancyRequirementsProspectsViewModelServerValidator.Validate(vacancyViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetVacancyRequirementsProspectsViewModel.FailedValidation, vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancyRequirementsProspectsViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyPartyViewModel> GetEmployerInformation(int vacancyReferenceNumber, bool? useEmployerLocation)
        {
            var vacancy = _vacancyQaProvider.GetNewVacancyViewModel(vacancyReferenceNumber);

            var viewModel = vacancy.OwnerParty;
            viewModel.IsEmployerLocationMainApprenticeshipLocation =
                    vacancy.IsEmployerLocationMainApprenticeshipLocation;
            viewModel.NumberOfPositions = vacancy.NumberOfPositions;
            viewModel.Status = vacancy.Status;
            viewModel.VacancyReferenceNumber = vacancy.VacancyReferenceNumber.Value;
            viewModel.EmployerDescriptionComment = vacancy.EmployerDescriptionComment;
            viewModel.EmployerWebsiteUrlComment = vacancy.EmployerWebsiteUrlComment;
            viewModel.NumberOfPositionsComment = vacancy.NumberOfPositionsComment;

            if (useEmployerLocation.HasValue && useEmployerLocation.Value)
            {
                viewModel.IsEmployerLocationMainApprenticeshipLocation = true;
            }

            var validationResult = _vacancyPartyViewModelValidator.Validate(vacancy.OwnerParty);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetEmployerInformation.FailedValidation,
                    vacancy.OwnerParty, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetEmployerInformation.Ok, vacancy.OwnerParty);
        }

        public MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel)
        {
            var validationResult = _vacancyQuestionsViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<NewVacancyViewModel> UpdateVacancy(NewVacancyViewModel viewModel)
        {
            var validationResult = _newVacancyViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<TrainingDetailsViewModel> UpdateVacancy(TrainingDetailsViewModel viewModel)
        {
            var validationResult = _trainingDetailsViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                var sectorsAndFrameworks = _vacancyQaProvider.GetSectorsAndFrameworks();
                var standards = _vacancyQaProvider.GetStandards();
                var sectors = _vacancyQaProvider.GetSectors();
                viewModel.SectorsAndFrameworks = sectorsAndFrameworks;
                viewModel.Standards = standards;
                viewModel.Sectors = sectors;

                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel)
        {
            var validationResult = _vacancyRequirementsProspectsViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel,
                    validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return
                GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<VacancyPartyViewModel> UpdateEmployerInformation(VacancyPartyViewModel viewModel)
        {
            var validationResult = _vacancyPartyViewModelValidator.Validate(viewModel);
            var existingVacancy = _vacancyQaProvider.GetNewVacancyViewModel(viewModel.VacancyReferenceNumber);

            var existingViewModel = existingVacancy.OwnerParty;
            existingViewModel.EmployerWebsiteUrl = viewModel.EmployerWebsiteUrl;
            existingViewModel.EmployerDescription = viewModel.EmployerDescription;
            existingViewModel.IsEmployerLocationMainApprenticeshipLocation =
                viewModel.IsEmployerLocationMainApprenticeshipLocation;
            existingViewModel.NumberOfPositions = viewModel.NumberOfPositions;
            existingViewModel.VacancyGuid = viewModel.VacancyGuid;
            existingViewModel.NumberOfPositionsComment = viewModel.NumberOfPositionsComment;
            existingViewModel.EmployerDescriptionComment = viewModel.EmployerDescriptionComment;
            existingViewModel.EmployerWebsiteUrlComment = viewModel.EmployerWebsiteUrlComment;

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.UpdateEmployerInformation.FailedValidation, existingViewModel, validationResult);
            }

            _providerQaProvider.ConfirmVacancyParty(viewModel);
            existingVacancy.IsEmployerLocationMainApprenticeshipLocation =
                viewModel.IsEmployerLocationMainApprenticeshipLocation;
            existingVacancy.NumberOfPositions = viewModel.NumberOfPositions;
            existingVacancy.VacancyGuid = viewModel.VacancyGuid;
            existingVacancy.NumberOfPositionsComment = viewModel.NumberOfPositionsComment;
            existingVacancy.EmployerDescriptionComment = viewModel.EmployerDescriptionComment;
            existingVacancy.EmployerWebsiteUrlComment = viewModel.EmployerWebsiteUrlComment;

            _vacancyQaProvider.UpdateEmployerInformationWithComments(existingVacancy);

            if (viewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue && viewModel.IsEmployerLocationMainApprenticeshipLocation.Value == true)
            {
                _vacancyQaProvider.RemoveLocationAddresses(viewModel.VacancyGuid);
            }
            
            return GetMediatorResponse(VacancyMediatorCodes.UpdateEmployerInformation.Ok, viewModel);
        }

        public MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyQaProvider.GetNewVacancyViewModel(vacancyReferenceNumber);

            var locationSearchViewModel = _vacancyQaProvider.LocationAddressesViewModel(vacancy.Ukprn,
                vacancy.OwnerParty.ProviderSiteId, vacancy.OwnerParty.Employer.EmployerId,
                vacancy.VacancyGuid);
            locationSearchViewModel.CurrentPage = 1;

            return GetMediatorResponse(VacancyMediatorCodes.GetLocationAddressesViewModel.Ok, locationSearchViewModel);
        }

        public MediatorResponse<LocationSearchViewModel> AddLocations(LocationSearchViewModel viewModel)
        {
            var validationResult = _locationSearchViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.AddLocations.FailedValidation, viewModel,
                    validationResult);
            }

            var locationSearchViewModel = _vacancyQaProvider.AddLocations(viewModel);

            return GetMediatorResponse(VacancyMediatorCodes.AddLocations.Ok, locationSearchViewModel);
        }

        public MediatorResponse<LocationSearchViewModel> SearchLocations(LocationSearchViewModel viewModel, List<VacancyLocationAddressViewModel> alreadyAddedLocations)
        {
            if (string.IsNullOrWhiteSpace(viewModel.PostcodeSearch))
            {
                AddAlreadyAddedLocations(viewModel, alreadyAddedLocations);

                return GetMediatorResponse(VacancyMediatorCodes.SearchLocations.NotFullPostcode, viewModel);
            }

            try
            {
                viewModel = _locationsProvider.GetAddressesFor(viewModel);

                AddAlreadyAddedLocations(viewModel, alreadyAddedLocations);

                return GetMediatorResponse(VacancyMediatorCodes.SearchLocations.Ok, viewModel);
            }
            catch (CustomException)
            {
                return GetMediatorResponse(VacancyMediatorCodes.SearchLocations.NotFullPostcode, viewModel);
            }
        }

        public MediatorResponse<LocationSearchViewModel> UseLocation(LocationSearchViewModel viewModel, int locationIndex, string postCodeSearch)
        {
            viewModel.PostcodeSearch = postCodeSearch;
            var searchResult = SearchLocations(viewModel, viewModel.Addresses);

            if (searchResult.ViewModel.SearchResultAddresses?.Page != null && searchResult.ViewModel.SearchResultAddresses.Page.Count() > locationIndex)
            {
                var addressToAdd = searchResult.ViewModel.SearchResultAddresses.Page.ToList()[locationIndex];

                var isNewAddress = viewModel.Addresses.TrueForAll(v => !v.Address.Equals(addressToAdd.Address));
                if (isNewAddress)
                {
                    viewModel.Addresses.Add(addressToAdd);
                }
            }

            viewModel.SearchResultAddresses = new PageableViewModel<VacancyLocationAddressViewModel>();
            viewModel.CurrentPage = 1;
            viewModel.TotalNumberOfPages = 1;
            viewModel.PostcodeSearch = string.Empty;

            return GetMediatorResponse(VacancyMediatorCodes.UseLocation.Ok, viewModel);
        }

        public MediatorResponse<LocationSearchViewModel> RemoveLocation(LocationSearchViewModel viewModel, int locationIndex)
        {
            viewModel.Addresses.RemoveAt(locationIndex);
            viewModel.SearchResultAddresses = new PageableViewModel<VacancyLocationAddressViewModel>();
            viewModel.PostcodeSearch = string.Empty;
            viewModel.CurrentPage = 1;
            viewModel.TotalNumberOfPages = 1;

            return GetMediatorResponse(VacancyMediatorCodes.RemoveLocation.Ok, viewModel);
        }

        private void AddAlreadyAddedLocations(LocationSearchViewModel viewModel, List<VacancyLocationAddressViewModel> alreadyAddedLocations)
        {
            var existingVacancy = _vacancyQaProvider.GetVacancy(viewModel.VacancyGuid);
            if (existingVacancy != null)
            {
                viewModel.Addresses = existingVacancy.LocationAddresses;
            }

            if (alreadyAddedLocations != null && alreadyAddedLocations.Any())
            {
                viewModel.Addresses = alreadyAddedLocations;
            }
        }
    }
}