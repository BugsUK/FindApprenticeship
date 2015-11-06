namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using System;
    using Raa.Common.Validators.Vacancy;
    using System.Linq;
    using FluentValidation;
    using Common.Constants;
    using Common.Mediators;
    using Common.Validators;
    using Common.ViewModels;
    using Raa.Common.Constants.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Providers;
    using Validators.VacancyPosting;
    using ViewModels.Vacancy;
    using Raa.Common.Validators.Provider;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.Converters;
    using Raa.Common.ViewModels.VacancyPosting;

    public class VacancyPostingMediator : MediatorBase, IVacancyPostingMediator
    {
        private readonly IVacancyPostingProvider _vacancyPostingProvider;
        private readonly IProviderProvider _providerProvider;
        private readonly IEmployerProvider _employerProvider;
        private readonly NewVacancyViewModelServerValidator _newVacancyViewModelServerValidator;
        private readonly NewVacancyViewModelClientValidator _newVacancyViewModelClientValidator;
        private readonly VacancySummaryViewModelServerValidator _vacancySummaryViewModelServerValidator;
        private readonly VacancySummaryViewModelClientValidator _vacancySummaryViewModelClientValidator;
        private readonly VacancyRequirementsProspectsViewModelServerValidator _vacancyRequirementsProspectsViewModelServerValidator;
        private readonly VacancyRequirementsProspectsViewModelClientValidator _vacancyRequirementsProspectsViewModelClientValidator;
        private readonly VacancyQuestionsViewModelServerValidator _vacancyQuestionsViewModelServerValidator;
        private readonly VacancyQuestionsViewModelClientValidator _vacancyQuestionsViewModelClientValidator;
        private readonly VacancyViewModelValidator _vacancyViewModelValidator;
        private readonly ProviderSiteEmployerLinkViewModelValidator _providerSiteEmployerLinkViewModelValidator;
        private readonly EmployerSearchViewModelServerValidator _employerSearchViewModelServerValidator;

        public VacancyPostingMediator(
            IVacancyPostingProvider vacancyPostingProvider,
            IProviderProvider providerProvider,
            IEmployerProvider employerProvider,
            NewVacancyViewModelServerValidator newVacancyViewModelServerValidator,
            NewVacancyViewModelClientValidator newVacancyViewModelClientValidator,
            VacancySummaryViewModelServerValidator vacancySummaryViewModelServerValidator,
            VacancySummaryViewModelClientValidator vacancySummaryViewModelClientValidator,
            VacancyRequirementsProspectsViewModelServerValidator vacancyRequirementsProspectsViewModelServerValidator,
            VacancyRequirementsProspectsViewModelClientValidator vacancyRequirementsProspectsViewModelClientValidator,
            VacancyQuestionsViewModelServerValidator vacancyQuestionsViewModelServerValidator,
            VacancyQuestionsViewModelClientValidator vacancyQuestionsViewModelClientValidator,
            VacancyViewModelValidator vacancyViewModelValidator,
            ProviderSiteEmployerLinkViewModelValidator providerSiteEmployerLinkViewModelValidator, EmployerSearchViewModelServerValidator employerSearchViewModelServerValidator)
        {
            _vacancyPostingProvider = vacancyPostingProvider;
            _providerProvider = providerProvider;
            _employerProvider = employerProvider;
            _newVacancyViewModelServerValidator = newVacancyViewModelServerValidator;
            _newVacancyViewModelClientValidator = newVacancyViewModelClientValidator;
            _vacancyViewModelValidator = vacancyViewModelValidator;
            _providerSiteEmployerLinkViewModelValidator = providerSiteEmployerLinkViewModelValidator;
            _employerSearchViewModelServerValidator = employerSearchViewModelServerValidator;
            _vacancySummaryViewModelServerValidator = vacancySummaryViewModelServerValidator;
            _vacancySummaryViewModelClientValidator = vacancySummaryViewModelClientValidator;
            _vacancyRequirementsProspectsViewModelServerValidator = vacancyRequirementsProspectsViewModelServerValidator;
            _vacancyRequirementsProspectsViewModelClientValidator = vacancyRequirementsProspectsViewModelClientValidator;
            _vacancyQuestionsViewModelServerValidator = vacancyQuestionsViewModelServerValidator;
            _vacancyQuestionsViewModelClientValidator = vacancyQuestionsViewModelClientValidator;
        }

        public MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(string providerSiteErn)
        {
            var viewModel = _providerProvider.GetProviderSiteEmployerLinkViewModels(providerSiteErn);

            var validationResult = _employerSearchViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.FailedValidation, viewModel, validationResult);
            }

            if ((viewModel.EmployerResults == null || !viewModel.EmployerResults.Any()) && (viewModel.EmployerResultsPage == null || viewModel.EmployerResultsPage.ResultsCount == 0))
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.NoResults, viewModel, EmployerSearchViewModelMessages.NoResultsText, UserMessageLevel.Info);
            }
            
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(EmployerSearchViewModel employerFilterViewModel)
        {
            var validationResult = _employerSearchViewModelServerValidator.Validate(employerFilterViewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.FailedValidation, employerFilterViewModel, validationResult);
            }

            //TODO: pull this into the view and use hidden form inputs
            //OR: just use a model that caters for it.
            if (!string.IsNullOrWhiteSpace(employerFilterViewModel.Ern))
            {
                employerFilterViewModel.FilterType = EmployerFilterType.Ern;
            }
            else if (!string.IsNullOrWhiteSpace(employerFilterViewModel.Location) || !string.IsNullOrWhiteSpace(employerFilterViewModel.Name))
            {
                employerFilterViewModel.FilterType = EmployerFilterType.NameAndLocation;
            }
            else
            {
                employerFilterViewModel.FilterType = EmployerFilterType.Undefined;
            }

            var viewModel = _providerProvider.GetProviderSiteEmployerLinkViewModels(employerFilterViewModel);

            if ((viewModel.EmployerResults == null || !viewModel.EmployerResults.Any()) && (viewModel.EmployerResultsPage == null || viewModel.EmployerResultsPage.ResultsCount == 0))
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.NoResults, viewModel, EmployerSearchViewModelMessages.NoResultsText, UserMessageLevel.Info);
            }

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<EmployerSearchViewModel> GetEmployers(EmployerSearchViewModel employerFilterViewModel)
        {
            var viewModel = _employerProvider.GetEmployerViewModels(employerFilterViewModel);
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployer(string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var viewModel = _providerProvider.GetProviderSiteEmployerLinkViewModel(providerSiteErn, ern);
            viewModel.VacancyGuid = vacancyGuid;
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetEmployer.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteEmployerLinkViewModel> ConfirmEmployer(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var validationResult = _providerSiteEmployerLinkViewModelValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                var existingViewModel = _providerProvider.GetProviderSiteEmployerLinkViewModel(viewModel.ProviderSiteErn, viewModel.Employer.Ern);
                existingViewModel.WebsiteUrl = viewModel.WebsiteUrl;
                existingViewModel.Description = viewModel.Description;

                return GetMediatorResponse(VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation, existingViewModel, validationResult);
            }

            var newViewModel = _providerProvider.ConfirmProviderSiteEmployerLink(viewModel);
            return GetMediatorResponse(VacancyPostingMediatorCodes.ConfirmEmployer.Ok, newViewModel);
        }

        public MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(string ukprn, string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var viewModel = _vacancyPostingProvider.GetNewVacancyViewModel(ukprn, providerSiteErn, ern, vacancyGuid);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetNewVacancyModel.Ok, viewModel);
        }

        public MediatorResponse<NewVacancyViewModel> GetNewVacancyViewModel(long vacancyReferenceNumber)
        {
            var viewModel = _vacancyPostingProvider.GetNewVacancyViewModel(vacancyReferenceNumber);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetNewVacancyModel.Ok, viewModel);
        }

        public MediatorResponse<NewVacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var validationResult = _newVacancyViewModelServerValidator.Validate(newVacancyViewModel);

            if (!validationResult.IsValid)
            {
                UpdateReferenceDataFor(newVacancyViewModel);

                return GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.FailedValidation, newVacancyViewModel, validationResult);
            }

            var storedVacancy = GetStoredVacancy(newVacancyViewModel);

            var createdVacancyViewModel = _vacancyPostingProvider.CreateVacancy(newVacancyViewModel);

            return SwitchingFromOnlineToOfflineVacancy(newVacancyViewModel, storedVacancy)
                ? GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.OkWithWarning, createdVacancyViewModel,
                    "TODO: questions will not appear on offline vacancies.", UserMessageLevel.Info)
                : GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.Ok, createdVacancyViewModel);
        }

        public MediatorResponse<NewVacancyViewModel> CreateVacancyAndExit(NewVacancyViewModel newVacancyViewModel)
        {
            var validationResult = _newVacancyViewModelClientValidator.Validate(newVacancyViewModel);

            if (!validationResult.IsValid)
            {
                UpdateReferenceDataFor(newVacancyViewModel);

                return GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.FailedValidation, newVacancyViewModel, validationResult);
            }

            var createdVacancyViewModel = _vacancyPostingProvider.CreateVacancy(newVacancyViewModel);

            return GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.Ok, createdVacancyViewModel);
        }

        private void UpdateReferenceDataFor(NewVacancyViewModel newVacancyViewModel)
        {
            newVacancyViewModel.SectorsAndFrameworks = _vacancyPostingProvider.GetSectorsAndFrameworks();
            newVacancyViewModel.Standards = _vacancyPostingProvider.GetStandards();
            if (newVacancyViewModel.TrainingType == TrainingType.Standards && newVacancyViewModel.StandardId.HasValue)
            {
                var standard = _vacancyPostingProvider.GetStandard(newVacancyViewModel.StandardId);
                newVacancyViewModel.ApprenticeshipLevel = standard?.ApprenticeshipLevel ?? ApprenticeshipLevel.Unknown;
            }
            newVacancyViewModel.ProviderSiteEmployerLink =
                _providerProvider.GetProviderSiteEmployerLinkViewModel(
                    newVacancyViewModel.ProviderSiteEmployerLink.ProviderSiteErn,
                    newVacancyViewModel.ProviderSiteEmployerLink.Employer.Ern);
        }

        private VacancyViewModel GetStoredVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            VacancyViewModel storedVacancy = null;

            if (newVacancyViewModel.VacancyReferenceNumber.HasValue)
            {
                storedVacancy = _vacancyPostingProvider.GetVacancy(newVacancyViewModel.VacancyReferenceNumber.Value);
            }

            return storedVacancy;
        }

        private static bool SwitchingFromOnlineToOfflineVacancy(NewVacancyViewModel newVacancyViewModel, VacancyViewModel existingVacancy)
        {
            return existingVacancy != null 
                && existingVacancy.NewVacancyViewModel.OfflineVacancy == false 
                && newVacancyViewModel.OfflineVacancy
                && ( !string.IsNullOrWhiteSpace(existingVacancy.VacancyQuestionsViewModel.FirstQuestion) || !string.IsNullOrWhiteSpace(existingVacancy.VacancyQuestionsViewModel.SecondQuestion));
        }

        public MediatorResponse<VacancySummaryViewModel> GetVacancySummaryViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancySummaryViewModel(vacancyReferenceNumber);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancySummaryViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancySummaryViewModel> UpdateVacancy(VacancySummaryViewModel viewModel, bool acceptWarnings)
        {
            var validationResult = _vacancySummaryViewModelServerValidator.Validate(viewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid && (!acceptWarnings || validationResult.Errors.Any(e => (ValidationType?)e.CustomState != ValidationType.Warning)))
            {
                viewModel.WageUnits = ApprenticeshipVacancyConverter.GetWageUnits();
                viewModel.DurationTypes = ApprenticeshipVacancyConverter.GetDurationTypes();

                return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyPostingProvider.UpdateVacancy(viewModel);

            return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<VacancySummaryViewModel> UpdateVacancyAndExit(VacancySummaryViewModel viewModel)
        {
            var validationResult = _vacancySummaryViewModelClientValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyPostingProvider.UpdateVacancy(viewModel);

            return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<VacancyRequirementsProspectsViewModel> GetVacancyRequirementsProspectsViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancyRequirementsProspectsViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancy(
            VacancyRequirementsProspectsViewModel viewModel)
        {
            var validationResult = _vacancyRequirementsProspectsViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation, viewModel,
                    validationResult);
            }

            var updatedViewModel = _vacancyPostingProvider.UpdateVacancy(viewModel);
            
            var completeViewModel = GetVacancyViewModel(viewModel.VacancyReferenceNumber);
            return
                GetMediatorResponse(
                    completeViewModel.ViewModel.NewVacancyViewModel.OfflineVacancy
                        ? VacancyPostingMediatorCodes.UpdateVacancy.OfflineVacancyOk
                        : VacancyPostingMediatorCodes.UpdateVacancy.OnlineVacancyOk, updatedViewModel);
        }

        public MediatorResponse<VacancyRequirementsProspectsViewModel> UpdateVacancyAndExit(
            VacancyRequirementsProspectsViewModel viewModel)
        {
            var validationResult = _vacancyRequirementsProspectsViewModelClientValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation, viewModel,
                    validationResult);
            }

            var updatedViewModel = _vacancyPostingProvider.UpdateVacancy(viewModel);

            return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.OkAndExit, updatedViewModel);
        }

        public MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancyQuestionsViewModel(vacancyReferenceNumber);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancyQuestionsViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyQuestionsViewModel> UpdateVacancy(VacancyQuestionsViewModel viewModel)
        {
            var validationResult = _vacancyQuestionsViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyPostingProvider.UpdateVacancy(viewModel);

            return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<VacancyQuestionsViewModel> UpdateVacancyAndExit(VacancyQuestionsViewModel viewModel)
        {
            var validationResult = _vacancyQuestionsViewModelClientValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation, viewModel, validationResult);
            }

            var updatedViewModel = _vacancyPostingProvider.UpdateVacancy(viewModel);

            return GetMediatorResponse(VacancyPostingMediatorCodes.UpdateVacancy.Ok, updatedViewModel);
        }

        public MediatorResponse<VacancyViewModel> GetVacancyViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancy(vacancyReferenceNumber);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancyViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyViewModel> GetPreviewVacancyViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancy(vacancyReferenceNumber);

            var validationResult = _vacancyViewModelValidator.Validate(vacancyViewModel, ruleSet:RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid)
            {
                //TODO: us its own mediator code
                return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancyViewModel.FailedValidation,
                    vacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancyViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyViewModel> SubmitVacancy(long vacancyReferenceNumber)
        {
            var vacancyViewModel =_vacancyPostingProvider.SubmitVacancy(vacancyReferenceNumber);

            return GetMediatorResponse(VacancyPostingMediatorCodes.SubmitVacancy.Ok, vacancyViewModel);
        }

        public MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancy(vacancyReferenceNumber);

            var viewModel = new SubmittedVacancyViewModel
            {
                VacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber,
                ProviderSiteErn = vacancyViewModel.NewVacancyViewModel.ProviderSiteEmployerLink.ProviderSiteErn
            };

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetSubmittedVacancyViewModel.Ok, viewModel);
        }

        public MediatorResponse<EmployerSearchViewModel> SelectNewEmployer(EmployerSearchViewModel viewModel)
        {
            if (viewModel.FilterType == EmployerFilterType.Undefined)
            {
                viewModel = new EmployerSearchViewModel
                {
                    ProviderSiteErn = viewModel.ProviderSiteErn,
                    FilterType = EmployerFilterType.Ern,
                    EmployerResults = Enumerable.Empty<EmployerResultViewModel>(),
                    EmployerResultsPage = new PageableViewModel<EmployerResultViewModel>()
                };
            }
            else
            {
                viewModel.EmployerResultsPage = viewModel.EmployerResultsPage ?? new PageableViewModel<EmployerResultViewModel>();

                var validationResult = _employerSearchViewModelServerValidator.Validate(viewModel);

                if (!validationResult.IsValid)
                {
                    return GetMediatorResponse(VacancyPostingMediatorCodes.SelectNewEmployer.FailedValidation, viewModel, validationResult);
                }

                viewModel = _employerProvider.GetEmployerViewModels(viewModel);

                if ((viewModel.EmployerResults == null || !viewModel.EmployerResults.Any()) && (viewModel.EmployerResultsPage == null || viewModel.EmployerResultsPage.ResultsCount == 0))
                {
                    return GetMediatorResponse(VacancyPostingMediatorCodes.SelectNewEmployer.NoResults, viewModel, EmployerSearchViewModelMessages.NoResultsErnRequiredText, UserMessageLevel.Info);
                }
            }

            return GetMediatorResponse(VacancyPostingMediatorCodes.SelectNewEmployer.Ok, viewModel, EmployerSearchViewModelMessages.ErnAdviceText, UserMessageLevel.Info);
        }
    }
}
