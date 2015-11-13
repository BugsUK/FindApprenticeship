namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using System.Linq;
    using Common.Mediators;
    using FluentValidation;
    using Common.Validators;
    using Raa.Common.Converters;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels;

    public class VacancyMediator : MediatorBase, IVacancyMediator
    {
        private readonly IVacancyQAProvider _vacancyQaProvider;

        private readonly VacancyViewModelValidator _vacancyViewModelValidator;
        private readonly VacancySummaryViewModelServerValidator _vacancySummaryViewModelServerValidator;
        private readonly NewVacancyViewModelServerValidator _newVacancyViewModelServerValidator;
        private readonly VacancyQuestionsViewModelServerValidator _vacancyQuestionsViewModelServerValidator;
        private readonly VacancyRequirementsProspectsViewModelServerValidator _vacancyRequirementsProspectsViewModelServerValidator;

        public VacancyMediator(IVacancyQAProvider vacancyQaProvider,
            VacancyViewModelValidator vacancyViewModelValidator,
            VacancySummaryViewModelServerValidator vacancySummaryViewModelServerValidator,
            NewVacancyViewModelServerValidator newVacancyViewModelServerValidator, 
            VacancyQuestionsViewModelServerValidator vacancyQuestionsViewModelServerValidator,
            VacancyRequirementsProspectsViewModelServerValidator vacancyRequirementsProspectsViewModelServerValidator)
        {
            _vacancyQaProvider = vacancyQaProvider;
            _vacancyViewModelValidator = vacancyViewModelValidator;
            _vacancySummaryViewModelServerValidator = vacancySummaryViewModelServerValidator;
            _newVacancyViewModelServerValidator = newVacancyViewModelServerValidator;
            _vacancyQuestionsViewModelServerValidator = vacancyQuestionsViewModelServerValidator;
            _vacancyRequirementsProspectsViewModelServerValidator = vacancyRequirementsProspectsViewModelServerValidator;
        }

        public MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(long vacancyReferenceNumber)
        {
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
            //TODO: basically the same code as in VacancyPostingMediator
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

            var validationResult = _newVacancyViewModelServerValidator.Validate(newVacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetBasicVacancyDetails.FailedValidation,
                    newVacancyViewModel, validationResult);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetBasicVacancyDetails.Ok, newVacancyViewModel);
        }

        public MediatorResponse<VacancyQuestionsViewModel> GetVacancyQuestionsViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyQaProvider.GetVacancyQuestionsViewModel(vacancyReferenceNumber);

            var validationResult = _vacancyQuestionsViewModelServerValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

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
            //TODO: basically the same code as in VacancyPostingMediator
            var validationResult = _newVacancyViewModelServerValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
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
    }
}