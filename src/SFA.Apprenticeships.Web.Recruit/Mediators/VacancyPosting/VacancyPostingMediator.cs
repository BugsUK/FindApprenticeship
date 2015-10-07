namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using System;
    using Common.Mediators;
    using Providers;
    using Validators.Vacancy;
    using Validators.VacancyPosting;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public class VacancyPostingMediator : MediatorBase, IVacancyPostingMediator
    {
        private readonly IVacancyPostingProvider _vacancyPostingProvider;
        private readonly IEmployerProvider _employerProvider;
        private readonly NewVacancyViewModelServerValidator _newVacancyViewModelServerValidator;
        private readonly VacancyViewModelValidator _vacancyViewModelValidator;

        public VacancyPostingMediator(
            IVacancyPostingProvider vacancyPostingProvider,
			IEmployerProvider employerProvider,
            NewVacancyViewModelServerValidator newVacancyViewModelServerValidator,
            VacancyViewModelValidator vacancyViewModelValidator)
        {
            _vacancyPostingProvider = vacancyPostingProvider;
			_employerProvider = employerProvider;
            _newVacancyViewModelServerValidator = newVacancyViewModelServerValidator;
            _vacancyViewModelValidator = vacancyViewModelValidator;
        }

        public MediatorResponse<EmployerResultsViewModel> GetProviderEmployers(Guid providerId, EmployerFilterViewModel employerFilterViewModel)
        {
            var viewModel = _employerProvider.GetEmployers(providerId, employerFilterViewModel);
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<EmployerResultsViewModel> GetEmployers(EmployerSearchViewModel employerFilterViewModel)
        {
            var viewModel = _employerProvider.GetEmployers(employerFilterViewModel);
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<NewVacancyViewModel> GetNewVacancyModel(string username)
        {
            var viewModel = _vacancyPostingProvider.GetNewVacancyViewModel(username);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetNewVacancyModel.Ok, viewModel);
        }

        public MediatorResponse<NewVacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var validationResult = _newVacancyViewModelServerValidator.Validate(newVacancyViewModel);

            if (!validationResult.IsValid)
            {
                newVacancyViewModel.SectorsAndFrameworks = _vacancyPostingProvider.GetSectorsAndFrameworks();

                return GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.FailedValidation, newVacancyViewModel, validationResult);
            }

            var createdVacancyViewModel = _vacancyPostingProvider.CreateVacancy(newVacancyViewModel);

            return GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.Ok, createdVacancyViewModel);
        }

        public MediatorResponse<VacancyViewModel> GetVacancyViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancy(vacancyReferenceNumber);

            var ern = "12345";//vacancyViewModel.ProviderSite.Ern;
            var employers = _employerProvider.GetEmployers(ern);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancyViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyViewModel> SubmitVacancy(VacancyViewModel viewModel)
        {
            var result = _vacancyViewModelValidator.Validate(viewModel);

            viewModel.ApprenticeshipLevels = _vacancyPostingProvider.GetApprenticeshipLevels();

            if (!result.IsValid)
            {
                return GetMediatorResponse(VacancyPostingMediatorCodes.SubmitVacancy.FailedValidation, viewModel, result);
            }

            return GetMediatorResponse(VacancyPostingMediatorCodes.SubmitVacancy.Ok, viewModel);
        }

        public MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber)
        {
            var viewModel = new SubmittedVacancyViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                ApproverEmail = "john.smith@salon-secrets.co.uk",
                PublishDate = new DateTime(2015, 10, 12)
            };

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetSubmittedVacancyViewModel.Ok, viewModel);
        }
    }
}
