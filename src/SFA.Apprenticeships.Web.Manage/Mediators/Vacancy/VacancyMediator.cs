namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using Common.Constants;
    using Common.Mediators;
    using Common.Validators;
    using Common.ViewModels;
    using Constants.ViewModels;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation;
    using Infrastructure.Presentation;
    using Raa.Common.Converters;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Provider;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.Validators.VacancyPosting;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyPosting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VacancyMediator : MediatorBase, IVacancyMediator
    {
        private readonly IVacancyQAProvider _vacancyQaProvider;
        private readonly IProviderQAProvider _providerQaProvider;
        private readonly ILocationsProvider _locationsProvider;

        private readonly VacancyViewModelValidator _vacancyViewModelValidator;
        private readonly VacancySummaryViewModelServerValidator _vacancySummaryViewModelServerValidator;
        private readonly NewVacancyViewModelClientValidator _newVacancyViewModelClientValidator = new NewVacancyViewModelClientValidator();
        private readonly NewVacancyViewModelServerValidator _newVacancyViewModelServerValidator;
        private readonly TrainingDetailsViewModelServerValidator _trainingDetailsViewModelServerValidator;
        private readonly VacancyQuestionsViewModelServerValidator _vacancyQuestionsViewModelServerValidator;
        private readonly VacancyRequirementsProspectsViewModelServerValidator _vacancyRequirementsProspectsViewModelServerValidator;
        private readonly VacancyOwnerRelationshipViewModelValidator _vacancyOwnerRelationshipViewModelValidator;
        private readonly LocationSearchViewModelServerValidator _locationSearchViewModelServerValidator;

        public VacancyMediator(IVacancyQAProvider vacancyQaProvider,
            VacancyViewModelValidator vacancyViewModelValidator,
            VacancySummaryViewModelServerValidator vacancySummaryViewModelServerValidator,
            NewVacancyViewModelServerValidator newVacancyViewModelServerValidator,
            VacancyQuestionsViewModelServerValidator vacancyQuestionsViewModelServerValidator,
            VacancyRequirementsProspectsViewModelServerValidator vacancyRequirementsProspectsViewModelServerValidator,
            VacancyOwnerRelationshipViewModelValidator vacancyOwnerRelationshipViewModelValidator,
            IProviderQAProvider providerQaProvider, LocationSearchViewModelServerValidator locationSearchViewModelServerValidator, ILocationsProvider locationsProvider, TrainingDetailsViewModelServerValidator trainingDetailsViewModelServerValidator)
        {
            _vacancyQaProvider = vacancyQaProvider;
            _vacancyViewModelValidator = vacancyViewModelValidator;
            _vacancySummaryViewModelServerValidator = vacancySummaryViewModelServerValidator;
            _newVacancyViewModelServerValidator = newVacancyViewModelServerValidator;
            _vacancyQuestionsViewModelServerValidator = vacancyQuestionsViewModelServerValidator;
            _vacancyRequirementsProspectsViewModelServerValidator = vacancyRequirementsProspectsViewModelServerValidator;
            _vacancyOwnerRelationshipViewModelValidator = vacancyOwnerRelationshipViewModelValidator;
            _providerQaProvider = providerQaProvider;
            _locationSearchViewModelServerValidator = locationSearchViewModelServerValidator;
            _locationsProvider = locationsProvider;
            _trainingDetailsViewModelServerValidator = trainingDetailsViewModelServerValidator;
        }

        public MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(int vacancyReferenceNumber)
        {
            var resultCode = _vacancyQaProvider.ApproveVacancy(vacancyReferenceNumber);

            switch (resultCode)
            {
                case QAActionResultCode.InvalidVacancy:
                    return
                        GetMediatorResponse<DashboardVacancySummaryViewModel>(
                            VacancyMediatorCodes.ApproveVacancy.InvalidVacancy, null,
                            VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);

                case QAActionResultCode.GeocodingFailure:
                    return
                        GetMediatorResponse<DashboardVacancySummaryViewModel>(
                            VacancyMediatorCodes.ApproveVacancy.PostcodeLookupFailed, null,
                            VacancyViewModelMessages.PostcodeLookupFailed, UserMessageLevel.Error);

            }

            var nextVacancy = _vacancyQaProvider.GetNextAvailableVacancy();

            return nextVacancy == null
                ? GetMediatorResponse<DashboardVacancySummaryViewModel>(
                    VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies, null, VacancyViewModelMessages.NoVacanciesAvailble, UserMessageLevel.Info)
                : GetMediatorResponse(VacancyMediatorCodes.ApproveVacancy.Ok, nextVacancy);
        }

        public MediatorResponse<DashboardVacancySummaryViewModel> RejectVacancy(int vacancyReferenceNumber)
        {
            if (_vacancyQaProvider.RejectVacancy(vacancyReferenceNumber) == QAActionResultCode.InvalidVacancy)
            {
                return
                    GetMediatorResponse<DashboardVacancySummaryViewModel>(
                        VacancyMediatorCodes.RejectVacancy.InvalidVacancy, null,
                        VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
            }

            var nextVacancy = _vacancyQaProvider.GetNextAvailableVacancy();

            return nextVacancy == null
                ? GetMediatorResponse<DashboardVacancySummaryViewModel>(
                    VacancyMediatorCodes.RejectVacancy.NoAvailableVacancies, null, VacancyViewModelMessages.NoVacanciesAvailble, UserMessageLevel.Info)
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
            vacancyViewModel.IsManageReviewerView = true;
            vacancyViewModel.IsEditable = vacancyViewModel.Status.IsStateReviewable();
            var validationResult = _vacancyViewModelValidator.Validate(vacancyViewModel);

            if (!validationResult.IsValid)
            {
                switch (vacancyViewModel.VacancySource)
                {
                    case VacancySource.Av:
                        return GetMediatorResponse(VacancyMediatorCodes.ReviewVacancy.VacancyAuthoredInAvmsWithValidationErrors,
                            vacancyViewModel, validationResult, VacancyViewModelMessages.VacancyAuthoredInAvms, UserMessageLevel.Info);
                    case VacancySource.Api:
                        return GetMediatorResponse(VacancyMediatorCodes.ReviewVacancy.VacancyAuthoredInApiWithValidationErrors,
                            vacancyViewModel, validationResult, VacancyViewModelMessages.VacancyAuthoredInApi, UserMessageLevel.Info);
                    case VacancySource.Raa:
                        return GetMediatorResponse(VacancyMediatorCodes.ReviewVacancy.FailedValidation,
                            vacancyViewModel, validationResult);
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            switch (vacancyViewModel.VacancySource)
            {
                case VacancySource.Av:
                    return GetMediatorResponse(VacancyMediatorCodes.ReviewVacancy.VacancyAuthoredInAvms, vacancyViewModel,
                        VacancyViewModelMessages.VacancyAuthoredInAvms, UserMessageLevel.Info);
                case VacancySource.Api:
                    return GetMediatorResponse(VacancyMediatorCodes.ReviewVacancy.VacancyAuthoredInApi, vacancyViewModel,
                        VacancyViewModelMessages.VacancyAuthoredInApi, UserMessageLevel.Info);
                case VacancySource.Raa:
                    return GetMediatorResponse(VacancyMediatorCodes.ReviewVacancy.Ok, vacancyViewModel);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public MediatorResponse<FurtherVacancyDetailsViewModel> GetVacancySummaryViewModel(int vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancySummaryViewModel(vacancyReferenceNumber);

            var validationResult = _vacancySummaryViewModelServerValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid)
            {
                vacancyViewModel.WageUnits = ApprenticeshipVacancyConverter.GetWageUnits();
                vacancyViewModel.WageTextPresets = ApprenticeshipVacancyConverter.GetWageTextPresets();
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
                viewModel.WageTextPresets = ApprenticeshipVacancyConverter.GetWageTextPresets();
                viewModel.DurationTypes = ApprenticeshipVacancyConverter.GetDurationTypes(viewModel.VacancyType);

                if (validationResult.Errors.All(e => (ValidationType?)e.CustomState == ValidationType.Warning))
                {
                    viewModel.AcceptWarnings = true;
                }

                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return ReturnResult(updatedViewModel);
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

        public MediatorResponse UnReserveVacancyForQA(int vacancyReferenceNumber)
        {
            _vacancyQaProvider.UnReserveVacancyForQA(vacancyReferenceNumber);

            return GetMediatorResponse(VacancyMediatorCodes.UnReserveVacancyForQA.Ok);
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

        public MediatorResponse<VacancyOwnerRelationshipViewModel> GetEmployerInformation(int vacancyReferenceNumber, bool? useEmployerLocation)
        {
            var vacancy = _vacancyQaProvider.GetNewVacancyViewModel(vacancyReferenceNumber);

            var viewModel = vacancy.VacancyOwnerRelationship;
            viewModel.IsEmployerLocationMainApprenticeshipLocation =
                    vacancy.IsEmployerLocationMainApprenticeshipLocation;
            viewModel.NumberOfPositions = vacancy.NumberOfPositions;
            viewModel.Status = vacancy.Status;
            viewModel.IsAnonymousEmployer = vacancy.VacancyOwnerRelationship.IsAnonymousEmployer;
            if (viewModel.IsAnonymousEmployer != null && viewModel.IsAnonymousEmployer.Value)
            {
                viewModel.AnonymousEmployerDescription = vacancy.VacancyOwnerRelationship.Employer.FullName;
                viewModel.AnonymousEmployerDescriptionComment = vacancy.AnonymousEmployerDescriptionComment;
                viewModel.AnonymousEmployerReason = vacancy.VacancyOwnerRelationship.AnonymousEmployerReason;
                viewModel.AnonymousEmployerReasonComment = vacancy.AnonymousEmployerReasonComment;
                viewModel.AnonymousAboutTheEmployer = vacancy.VacancyOwnerRelationship.AnonymousAboutTheEmployer;
                viewModel.AnonymousAboutTheEmployerComment = vacancy.AnonymousAboutTheEmployerComment;
            }
            else
            {
                viewModel.EmployerDescriptionComment = vacancy.EmployerDescriptionComment;
                viewModel.EmployerWebsiteUrlComment = vacancy.EmployerWebsiteUrlComment;
            }

            if (vacancy.VacancyReferenceNumber.HasValue)
            {
                viewModel.VacancyReferenceNumber = vacancy.VacancyReferenceNumber.Value;
            }


            viewModel.NumberOfPositionsComment = vacancy.NumberOfPositionsComment;

            if (useEmployerLocation.HasValue && useEmployerLocation.Value)
            {
                viewModel.IsEmployerLocationMainApprenticeshipLocation = true;
            }

            var validationResult = _vacancyOwnerRelationshipViewModelValidator.Validate(vacancy.VacancyOwnerRelationship);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetEmployerInformation.FailedValidation,
                    vacancy.VacancyOwnerRelationship, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetEmployerInformation.Ok, vacancy.VacancyOwnerRelationship);
        }

        private static MediatorResponse<T> ReturnResult<T>(QAActionResult<T> result) where T : IPartialVacancyViewModel
        {
            switch (result.Code)
            {
                case QAActionResultCode.Ok:
                    return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.Ok, result.ViewModel);
                case QAActionResultCode.InvalidVacancy:
                    return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.InvalidVacancy, default(T),
                        VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel)
        {
            var validationResult = _vacancyQuestionsViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return ReturnResult(updatedViewModel);
        }

        public MediatorResponse<NewVacancyViewModel> UpdateVacancy(NewVacancyViewModel viewModel)
        {
            return UpdateVacancy(viewModel, _newVacancyViewModelServerValidator);
        }

        public MediatorResponse<NewVacancyViewModel> UpdateOfflineVacancyType(NewVacancyViewModel viewModel)
        {
            return UpdateVacancy(viewModel, _newVacancyViewModelClientValidator);
        }

        public MediatorResponse<NewVacancyViewModel> UpdateVacancy(NewVacancyViewModel viewModel, AbstractValidator<NewVacancyViewModel> validator)
        {
            var validationResult = validator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                var locationAddresses = _vacancyQaProvider.GetLocationsAddressViewModelsByReferenceNumber(viewModel.VacancyReferenceNumber.Value);
                if (viewModel.LocationAddresses != null)
                {
                    foreach (var locationAddress in locationAddresses)
                    {
                        locationAddress.OfflineApplicationUrl = viewModel.LocationAddresses.SingleOrDefault(la => la.VacancyLocationId == locationAddress.VacancyLocationId)?.OfflineApplicationUrl;
                    }
                }
                viewModel.LocationAddresses = locationAddresses;
                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return ReturnResult(updatedViewModel);
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

            return ReturnResult(updatedViewModel);
        }

        public MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(VacancyRequirementsProspectsViewModel viewModel)
        {
            var validationResult = _vacancyRequirementsProspectsViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return ReturnResult(updatedViewModel);
        }

        public MediatorResponse<VacancyOwnerRelationshipViewModel> UpdateEmployerInformation(VacancyOwnerRelationshipViewModel viewModel)
        {
            var validationResult = _vacancyOwnerRelationshipViewModelValidator.Validate(viewModel);
            var existingVacancy = _vacancyQaProvider.GetNewVacancyViewModel(viewModel.VacancyReferenceNumber);

            var existingViewModel = existingVacancy.VacancyOwnerRelationship;
            existingViewModel.EmployerWebsiteUrl = viewModel.EmployerWebsiteUrl;
            existingViewModel.EmployerDescription = viewModel.EmployerDescription;
            existingViewModel.IsEmployerLocationMainApprenticeshipLocation = viewModel.IsEmployerLocationMainApprenticeshipLocation;
            existingViewModel.NumberOfPositions = viewModel.NumberOfPositions;
            existingViewModel.VacancyGuid = viewModel.VacancyGuid;
            existingViewModel.NumberOfPositionsComment = viewModel.NumberOfPositionsComment;
            existingViewModel.EmployerDescriptionComment = viewModel.EmployerDescriptionComment;
            existingViewModel.EmployerWebsiteUrlComment = viewModel.EmployerWebsiteUrlComment;
            existingViewModel.IsAnonymousEmployer = viewModel.IsAnonymousEmployer;
            existingViewModel.AnonymousEmployerReason = viewModel.AnonymousEmployerReason;
            existingViewModel.AnonymousEmployerDescription = viewModel.AnonymousEmployerDescription;
            existingViewModel.AnonymousAboutTheEmployer = viewModel.AnonymousAboutTheEmployer;
            existingViewModel.AnonymousEmployerReasonComment = viewModel.AnonymousEmployerReasonComment;
            existingViewModel.AnonymousEmployerDescriptionComment = viewModel.AnonymousEmployerDescriptionComment;
            existingViewModel.AnonymousAboutTheEmployerComment = viewModel.AnonymousAboutTheEmployerComment;

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.UpdateEmployerInformation.FailedValidation, existingViewModel, validationResult);
            }

            _providerQaProvider.ConfirmVacancyOwnerRelationship(viewModel);

            existingVacancy.IsEmployerLocationMainApprenticeshipLocation = viewModel.IsEmployerLocationMainApprenticeshipLocation;
            existingVacancy.NumberOfPositions = viewModel.NumberOfPositions;
            existingVacancy.VacancyGuid = viewModel.VacancyGuid;
            existingVacancy.NumberOfPositionsComment = viewModel.NumberOfPositionsComment;
            existingVacancy.EmployerDescriptionComment = viewModel.EmployerDescriptionComment;
            existingVacancy.EmployerWebsiteUrlComment = viewModel.EmployerWebsiteUrlComment;
            existingVacancy.IsAnonymousEmployer = viewModel.IsAnonymousEmployer;
            existingVacancy.AnonymousEmployerReason = viewModel.AnonymousEmployerReason;
            existingVacancy.AnonymousEmployerDescription = viewModel.AnonymousEmployerDescription;
            existingVacancy.AnonymousAboutTheEmployer = viewModel.AnonymousAboutTheEmployer;
            existingVacancy.AnonymousEmployerReasonComment = viewModel.AnonymousEmployerReasonComment;
            existingVacancy.AnonymousEmployerDescriptionComment = viewModel.AnonymousEmployerDescriptionComment;
            existingVacancy.AnonymousAboutTheEmployerComment = viewModel.AnonymousAboutTheEmployerComment;

            _vacancyQaProvider.UpdateEmployerInformationWithComments(existingVacancy);

            if (viewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue && viewModel.IsEmployerLocationMainApprenticeshipLocation.Value)
            {
                _vacancyQaProvider.RemoveLocationAddresses(viewModel.VacancyGuid);
            }

            return GetMediatorResponse(VacancyMediatorCodes.UpdateEmployerInformation.Ok, viewModel);
        }

        public MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(int vacancyReferenceNumber)
        {
            var vacancy = _vacancyQaProvider.GetNewVacancyViewModel(vacancyReferenceNumber);

            var locationSearchViewModel = _vacancyQaProvider.LocationAddressesViewModel(vacancy.Ukprn, vacancy.VacancyOwnerRelationship.ProviderSiteId, vacancy.VacancyOwnerRelationship.Employer.EmployerId, vacancy.VacancyGuid);
            locationSearchViewModel.CurrentPage = 1;

            return GetMediatorResponse(VacancyMediatorCodes.GetLocationAddressesViewModel.Ok, locationSearchViewModel);
        }

        public MediatorResponse<LocationSearchViewModel> AddLocations(LocationSearchViewModel viewModel)
        {
            var validationResult = _locationSearchViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.AddLocations.FailedValidation, viewModel, validationResult);
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