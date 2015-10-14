namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using System.Linq;
    using Common.Mediators;
    using Converters;
    using Providers;
    using Validators.Provider;
    using Validators.Vacancy;
    using Validators.VacancyPosting;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public class VacancyPostingMediator : MediatorBase, IVacancyPostingMediator
    {
        private readonly IVacancyPostingProvider _vacancyPostingProvider;
        private readonly IProviderProvider _providerProvider;
        private readonly IEmployerProvider _employerProvider;
        private readonly NewVacancyViewModelServerValidator _newVacancyViewModelServerValidator;
        private readonly VacancyViewModelValidator _vacancyViewModelValidator;
        private readonly ProviderSiteEmployerLinkViewModelValidator _providerSiteEmployerLinkViewModelValidator;

        public VacancyPostingMediator(
            IVacancyPostingProvider vacancyPostingProvider,
            IProviderProvider providerProvider,
            IEmployerProvider employerProvider,
            NewVacancyViewModelServerValidator newVacancyViewModelServerValidator,
            VacancyViewModelValidator vacancyViewModelValidator,
            ProviderSiteEmployerLinkViewModelValidator providerSiteEmployerLinkViewModelValidator)
        {
            _vacancyPostingProvider = vacancyPostingProvider;
            _providerProvider = providerProvider;
            _employerProvider = employerProvider;
            _newVacancyViewModelServerValidator = newVacancyViewModelServerValidator;
            _vacancyViewModelValidator = vacancyViewModelValidator;
            _providerSiteEmployerLinkViewModelValidator = providerSiteEmployerLinkViewModelValidator;
        }

        public MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(string providerSiteErn)
        {
            var viewModels = _providerProvider.GetProviderSiteEmployerLinkViewModels(providerSiteErn);
            var viewModel = new EmployerSearchViewModel
            {
                ProviderSiteErn = providerSiteErn,
                EmployerResults = viewModels.Select(vm => vm.Employer.ConvertToResult())
            };
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<EmployerSearchViewModel> GetProviderEmployers(EmployerSearchViewModel employerFilterViewModel)
        {
            var viewModels = _providerProvider.GetProviderSiteEmployerLinkViewModels(employerFilterViewModel);
            employerFilterViewModel.EmployerResults = viewModels.Select(vm => vm.Employer.ConvertToResult());
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, employerFilterViewModel);
        }

        public MediatorResponse<EmployerSearchViewModel> GetEmployers(EmployerSearchViewModel employerFilterViewModel)
        {
            var viewModel = _employerProvider.GetEmployerViewModels(employerFilterViewModel);
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetProviderEmployers.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteEmployerLinkViewModel> GetEmployer(string providerSiteErn, string ern)
        {
            var viewModel = _providerProvider.GetProviderSiteEmployerLinkViewModel(providerSiteErn, ern);
            return GetMediatorResponse(VacancyPostingMediatorCodes.GetEmployer.Ok, viewModel);
        }

        public MediatorResponse<ProviderSiteEmployerLinkViewModel> ConfirmEmployer(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var validationResult = _providerSiteEmployerLinkViewModelValidator.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                var existingViewModel = _providerProvider.GetProviderSiteEmployerLinkViewModel(viewModel.ProviderSiteErn, viewModel.Employer.Ern);
                existingViewModel.Description = viewModel.Description;

                return GetMediatorResponse(VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation, existingViewModel, validationResult);
            }

            var newViewModel = _providerProvider.ConfirmProviderSiteEmployerLink(viewModel);
            return GetMediatorResponse(VacancyPostingMediatorCodes.ConfirmEmployer.Ok, newViewModel);
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
                newVacancyViewModel.ProviderSiteEmployerLink = _providerProvider.GetProviderSiteEmployerLinkViewModel(newVacancyViewModel.ProviderSiteEmployerLink.ProviderSiteErn, newVacancyViewModel.ProviderSiteEmployerLink.Employer.Ern);

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
                var existingViewModel = _vacancyPostingProvider.GetVacancy(viewModel.VacancyReferenceNumber);

                viewModel.ApprenticeshipLevels = existingViewModel.ApprenticeshipLevels;
                viewModel.FrameworkName = existingViewModel.FrameworkName;
                viewModel.ProviderSiteEmployerLink = existingViewModel.ProviderSiteEmployerLink;
                viewModel.ProviderSite = existingViewModel.ProviderSite;

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
                ProviderSiteErn = vacancyViewModel.ProviderSiteEmployerLink.ProviderSiteErn
            };

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetSubmittedVacancyViewModel.Ok, viewModel);
        }
    }
}
