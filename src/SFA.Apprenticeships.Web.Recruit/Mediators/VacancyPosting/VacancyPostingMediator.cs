namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Mediators;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Providers;
    using Validators.Vacancy;
    using ViewModels.Vacancy;

    public class VacancyPostingMediator : MediatorBase, IVacancyPostingMediator
    {
        private readonly IVacancyPostingProvider _vacancyPostingProvider;
        private readonly IEmployerProvider _employerProvider;
        private readonly VacancyViewModelValidator _vacancyViewModelValidator;

        public VacancyPostingMediator(IVacancyPostingProvider vacancyPostingProvider, IEmployerProvider employerProvider, VacancyViewModelValidator vacancyViewModelValidator)
        {
            _vacancyPostingProvider = vacancyPostingProvider;
            _employerProvider = employerProvider;
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

            var ern = "12345";//vacancyViewModel.ProviderSite.Ern;
            var employers = _employerProvider.GetEmployers(ern);

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

        // TODO: AG: US811: this function is duplicated elsewhere.
        private static List<SelectListItem> GetApprenticeshipLevels()
        {
            var levels =
                Enum.GetValues(typeof(ApprenticeshipLevel))
                    .Cast<ApprenticeshipLevel>()
                    .Where(al => al != ApprenticeshipLevel.Unknown)
                    .Select(al => new SelectListItem { Value = al.ToString(), Text = al.ToString() })
                    .ToList();

            return levels;
        }
    }
}
