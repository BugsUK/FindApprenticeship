namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using System;
    using Common.Mediators;
    using Providers;
    using Validators.Vacancy;
    using ViewModels.Vacancy;

    public class VacancyPostingMediator : MediatorBase, IVacancyPostingMediator
    {
        private readonly IVacancyPostingProvider _vacancyPostingProvider;
        private readonly VacancyViewModelValidator _vacancyViewModelValidator;

        public VacancyPostingMediator(IVacancyPostingProvider vacancyPostingProvider, VacancyViewModelValidator vacancyViewModelValidator)
        {
            _vacancyPostingProvider = vacancyPostingProvider;
            _vacancyViewModelValidator = vacancyViewModelValidator;
        }

        public MediatorResponse<NewVacancyViewModel> GetNewVacancyModel(string username)
        {
            var viewModel = _vacancyPostingProvider.GetNewVacancyViewModel(username);

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetNewVacancyModel.Ok, viewModel);
        }

        public MediatorResponse<VacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel)
        {
            // TODO: AG: US811: implement validation.
            // var result = _newVacancyViewModelValidator.Validate(viewModel)
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

            viewModel.ApprenticeshipLevels = GetApprenticeshipLevels();

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
