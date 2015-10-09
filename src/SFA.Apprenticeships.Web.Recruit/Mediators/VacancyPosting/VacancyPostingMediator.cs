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

        public MediatorResponse<EmployerFilterViewModel> GetProviderEmployers(EmployerFilterViewModel employerFilterViewModel)
        {
            var viewModel = _employerProvider.GetEmployerViewModels(employerFilterViewModel);
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<EmployerSearchViewModel> GetEmployers(EmployerSearchViewModel employerFilterViewModel)
        {
            var viewModel = _employerProvider.GetEmployerViewModels(employerFilterViewModel);
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<EmployerViewModel> GetEmployer(string providerSiteErn, string ern)
        {
            var viewModel = _employerProvider.GetEmployerViewModel(providerSiteErn, ern);
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetEmployer.Ok, viewModel);
        }

        public MediatorResponse<EmployerViewModel> ConfirmEmployer(string providerSiteErn, string ern, string description)
        {
            var viewModel = _employerProvider.ConfirmEmployer(providerSiteErn, ern, description);
            return GetMediatorResponse(VacancyPostingMediatorCodes.ConfirmEmployer.Ok, viewModel);
        }

        public MediatorResponse<NewVacancyViewModel> GetNewVacancyModel(string ukprn, string providerSiteErn, string ern)
        {
            var viewModel = _vacancyPostingProvider.GetNewVacancyViewModel(ukprn, providerSiteErn, ern);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetNewVacancyModel.Ok, viewModel);
        }

        public MediatorResponse<NewVacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            var validationResult = _newVacancyViewModelServerValidator.Validate(newVacancyViewModel);

            if (!validationResult.IsValid)
            {
                newVacancyViewModel.SectorsAndFrameworks = _vacancyPostingProvider.GetSectorsAndFrameworks();
                newVacancyViewModel.Employer = _employerProvider.GetEmployerViewModel(newVacancyViewModel.Employer.ProviderSiteErn, newVacancyViewModel.Employer.Ern);

                return GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.FailedValidation, newVacancyViewModel, validationResult);
            }

            var createdVacancyViewModel = _vacancyPostingProvider.CreateVacancy(newVacancyViewModel);

            return GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.Ok, createdVacancyViewModel);
        }

        public MediatorResponse<VacancyViewModel> GetVacancyViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancy(vacancyReferenceNumber);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancyViewModel.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyViewModel> SubmitVacancy(VacancyViewModel viewModel)
        {
            var result = _vacancyViewModelValidator.Validate(viewModel);

            if (!result.IsValid)
            {
                viewModel = _vacancyPostingProvider.GetVacancy(viewModel.VacancyReferenceNumber);

                return GetMediatorResponse(VacancyPostingMediatorCodes.SubmitVacancy.FailedValidation, viewModel, result);
            }

            viewModel = _vacancyPostingProvider.SubmitVacancy(viewModel);
            
            return GetMediatorResponse(VacancyPostingMediatorCodes.SubmitVacancy.Ok, viewModel);
        }

        public MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyPostingProvider.GetVacancy(vacancyReferenceNumber);

            var viewModel = new SubmittedVacancyViewModel
            {
                VacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber,
                ApproverEmail = "where_does_this_come_from@requirements.com",
                PublishDate = vacancyViewModel.PublishDate ?? DateTime.MinValue,
                ProviderSiteErn = vacancyViewModel.Employer.ProviderSiteErn
            };

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetSubmittedVacancyViewModel.Ok, viewModel);
        }
    }
}
