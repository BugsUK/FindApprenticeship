namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Mediators;
    using FluentValidation;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
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
        private readonly ProviderSiteEmployerLinkViewModelValidator _providerSiteEmployerLinkViewModelValidator;
        private readonly LocationSearchViewModelValidator _locationSearchViewModelValidator;

        public VacancyMediator(IVacancyQAProvider vacancyQaProvider,
            VacancyViewModelValidator vacancyViewModelValidator,
            VacancySummaryViewModelServerValidator vacancySummaryViewModelServerValidator,
            NewVacancyViewModelServerValidator newVacancyViewModelServerValidator, 
            VacancyQuestionsViewModelServerValidator vacancyQuestionsViewModelServerValidator,
            VacancyRequirementsProspectsViewModelServerValidator vacancyRequirementsProspectsViewModelServerValidator, 
            ProviderSiteEmployerLinkViewModelValidator providerSiteEmployerLinkViewModelValidator, 
            IProviderQAProvider providerQaProvider, LocationSearchViewModelValidator locationSearchViewModelValidator, ILocationsProvider locationsProvider, TrainingDetailsViewModelServerValidator trainingDetailsViewModelServerValidator)
        {
            _vacancyQaProvider = vacancyQaProvider;
            _vacancyViewModelValidator = vacancyViewModelValidator;
            _vacancySummaryViewModelServerValidator = vacancySummaryViewModelServerValidator;
            _newVacancyViewModelServerValidator = newVacancyViewModelServerValidator;
            _vacancyQuestionsViewModelServerValidator = vacancyQuestionsViewModelServerValidator;
            _vacancyRequirementsProspectsViewModelServerValidator = vacancyRequirementsProspectsViewModelServerValidator;
            _providerSiteEmployerLinkViewModelValidator = providerSiteEmployerLinkViewModelValidator;
            _providerQaProvider = providerQaProvider;
            _locationSearchViewModelValidator = locationSearchViewModelValidator;
            _locationsProvider = locationsProvider;
            _trainingDetailsViewModelServerValidator = trainingDetailsViewModelServerValidator;
        }

        public MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(long vacancyReferenceNumber)
        {
            //TODO: There should be validation here
            _vacancyQaProvider.ApproveVacancy(vacancyReferenceNumber);

            var vacancies = _vacancyQaProvider.GetPendingQAVacancies();

            if (vacancies == null || !vacancies.Any())
            {
                return GetMediatorResponse<DashboardVacancySummaryViewModel>(VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies);
            }

            return GetMediatorResponse(VacancyMediatorCodes.ApproveVacancy.Ok, vacancies.First());
        }

        public MediatorResponse<DashboardVacancySummaryViewModel> RejectVacancy(long vacancyReferenceNumber)
        {
            //TODO: There should be validation here
            _vacancyQaProvider.RejectVacancy(vacancyReferenceNumber);

            var vacancies = _vacancyQaProvider.GetPendingQAVacancies();

            if (vacancies == null || !vacancies.Any())
            {
                return GetMediatorResponse<DashboardVacancySummaryViewModel>(VacancyMediatorCodes.RejectVacancy.NoAvailableVacancies);
            }

            return GetMediatorResponse(VacancyMediatorCodes.RejectVacancy.Ok, vacancies.First());
        }

        public MediatorResponse<VacancyViewModel> ReserveVacancyForQA(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.ReserveVacancyForQA(vacancyReferenceNumber);

            var validationResult = _vacancyViewModelValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetVacancy.FailedValidation,
                    vacancyViewModel, validationResult);
            }

            if (vacancyViewModel == null)
            {
                return GetMediatorResponse<VacancyViewModel>(VacancyMediatorCodes.GetVacancy.NotAvailable);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancy.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancySummaryViewModel(vacancyReferenceNumber);

            var validationResult = _vacancySummaryViewModelServerValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid )
            {
                vacancyViewModel.WageUnits = ApprenticeshipVacancyConverter.GetWageUnits();
                vacancyViewModel.DurationTypes = ApprenticeshipVacancyConverter.GetDurationTypes();

                return GetMediatorResponse(VacancyMediatorCodes.GetVacancySummaryViewModel.FailedValidation, vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancySummaryViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel)
        {
            var validationResult = _vacancySummaryViewModelServerValidator.Validate(viewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid && (!viewModel.AcceptWarnings || validationResult.Errors.Any(e => (ValidationType?)e.CustomState != ValidationType.Warning)))
            {
                viewModel.WageUnits = ApprenticeshipVacancyConverter.GetWageUnits();
                viewModel.DurationTypes = ApprenticeshipVacancyConverter.GetDurationTypes();

                viewModel.AcceptWarnings = true;

                return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyQaProvider.UpdateVacancyWithComments(viewModel);

            return GetMediatorResponse(VacancyMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<NewVacancyViewModel> GetBasicDetails(long vacancyReferenceNumber)
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

        public MediatorResponse<TrainingDetailsViewModel> GetTrainingDetails(long vacancyReferenceNumber)
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
            viewModel.Standards = _vacancyQaProvider.GetStandards();
            viewModel.SectorsAndFrameworks = _vacancyQaProvider.GetSectorsAndFrameworks();

            return GetMediatorResponse(VacancyMediatorCodes.SelectFrameworkAsTrainingType.Ok, viewModel);
        }

        public MediatorResponse<TrainingDetailsViewModel> SelectStandardAsTrainingType(TrainingDetailsViewModel viewModel)
        {
            viewModel.TrainingType = TrainingType.Standards;
            viewModel.StandardId = null;
            viewModel.ApprenticeshipLevel = ApprenticeshipLevel.Unknown;
            viewModel.Standards = _vacancyQaProvider.GetStandards();
            viewModel.SectorsAndFrameworks = _vacancyQaProvider.GetSectorsAndFrameworks();

            return GetMediatorResponse(VacancyMediatorCodes.SelectStandardAsTrainingType.Ok, viewModel);
        }

        public MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancyQuestionsViewModel(vacancyReferenceNumber);

            var validationResult = _vacancyQuestionsViewModelServerValidator.Validate(vacancyViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetVacancyQuestionsViewModel.FailedValidation, vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancyQuestionsViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber);

            var validationResult = _vacancyRequirementsProspectsViewModelServerValidator.Validate(vacancyViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetVacancyRequirementsProspectsViewModel.FailedValidation, vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancyRequirementsProspectsViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployerInformation(long vacancyReferenceNumber, bool? useEmployerLocation)
        {
            var vacancy = _vacancyQaProvider.GetNewVacancyViewModel(vacancyReferenceNumber);

            var viewModel = vacancy.ProviderSiteEmployerLink;
            viewModel.IsEmployerLocationMainApprenticeshipLocation =
                    vacancy.IsEmployerLocationMainApprenticeshipLocation;
            viewModel.NumberOfPositions = vacancy.NumberOfPositions;
            viewModel.Status = vacancy.Status;
            viewModel.VacancyReferenceNumber = vacancy.VacancyReferenceNumber.Value;
            viewModel.DescriptionComment = vacancy.EmployerDescriptionComment;
            viewModel.WebsiteUrlComment = vacancy.EmployerWebsiteUrlComment;
            viewModel.NumberOfPositionsComment = vacancy.NumberOfPositionsComment;

            if (useEmployerLocation.HasValue && useEmployerLocation.Value)
            {
                viewModel.IsEmployerLocationMainApprenticeshipLocation = true;
            }

            var validationResult = _providerSiteEmployerLinkViewModelValidator.Validate(vacancy.ProviderSiteEmployerLink);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetEmployerInformation.FailedValidation,
                    vacancy.ProviderSiteEmployerLink, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetEmployerInformation.Ok, vacancy.ProviderSiteEmployerLink);
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
                var sectors = _vacancyQaProvider.GetSectorsAndFrameworks();
                var standards = _vacancyQaProvider.GetStandards();
                viewModel.SectorsAndFrameworks = sectors;
                viewModel.Standards = standards;

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

        public MediatorResponse<ProviderSiteEmployerLinkViewModel> UpdateEmployerInformation(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var validationResult = _providerSiteEmployerLinkViewModelValidator.Validate(viewModel);
            var existingVacancy = _vacancyQaProvider.GetNewVacancyViewModel(viewModel.VacancyReferenceNumber);

            var existingViewModel = existingVacancy.ProviderSiteEmployerLink;
            existingViewModel.WebsiteUrl = viewModel.WebsiteUrl;
            existingViewModel.Description = viewModel.Description;
            existingViewModel.IsEmployerLocationMainApprenticeshipLocation =
                viewModel.IsEmployerLocationMainApprenticeshipLocation;
            existingViewModel.NumberOfPositions = viewModel.NumberOfPositions;
            existingViewModel.VacancyGuid = viewModel.VacancyGuid;
            existingViewModel.NumberOfPositionsComment = viewModel.NumberOfPositionsComment;
            existingViewModel.DescriptionComment = viewModel.DescriptionComment;
            existingViewModel.WebsiteUrlComment = viewModel.WebsiteUrlComment;

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.UpdateEmployerInformation.FailedValidation, existingViewModel, validationResult);
            }

            _providerQaProvider.ConfirmProviderSiteEmployerLink(viewModel);
            existingVacancy.IsEmployerLocationMainApprenticeshipLocation =
                viewModel.IsEmployerLocationMainApprenticeshipLocation;
            existingVacancy.NumberOfPositions = viewModel.NumberOfPositions;
            existingVacancy.VacancyGuid = viewModel.VacancyGuid;
            existingVacancy.NumberOfPositionsComment = viewModel.NumberOfPositionsComment;
            existingVacancy.EmployerDescriptionComment = viewModel.DescriptionComment;
            existingVacancy.EmployerWebsiteUrlComment = viewModel.WebsiteUrlComment;

            _vacancyQaProvider.UpdateEmployerInformationWithComments(existingVacancy);

            if (viewModel.IsEmployerLocationMainApprenticeshipLocation.HasValue && viewModel.IsEmployerLocationMainApprenticeshipLocation.Value == true)
            {
                _vacancyQaProvider.RemoveLocationAddresses(viewModel.VacancyGuid);
            }
            
            return GetMediatorResponse(VacancyMediatorCodes.UpdateEmployerInformation.Ok, viewModel);
        }

        public MediatorResponse<LocationSearchViewModel> GetLocationAddressesViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyQaProvider.GetNewVacancyViewModel(vacancyReferenceNumber);

            var locationSearchViewModel = _vacancyQaProvider.LocationAddressesViewModel(vacancy.Ukprn,
                vacancy.ProviderSiteEmployerLink.ProviderSiteErn, vacancy.ProviderSiteEmployerLink.Employer.Ern,
                vacancy.VacancyGuid);
            locationSearchViewModel.CurrentPage = 1;

            return GetMediatorResponse(VacancyMediatorCodes.GetLocationAddressesViewModel.Ok, locationSearchViewModel);
        }

        public MediatorResponse<LocationSearchViewModel> AddLocations(LocationSearchViewModel viewModel)
        {
            var validationResult = _locationSearchViewModelValidator.Validate(viewModel);

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