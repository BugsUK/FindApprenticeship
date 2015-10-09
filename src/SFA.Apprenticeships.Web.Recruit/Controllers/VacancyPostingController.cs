namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Controllers;
    using Common.Mediators;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators.VacancyPosting;
    using Providers;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    [AuthorizationData]
    [AuthorizeUser(Roles = Roles.Faa)]
    [OwinSessionTimeout]
    public class VacancyPostingController : ControllerBase<RecruitmentUserContext>
    {
        private readonly IVacancyPostingMediator _vacancyPostingMediator;

        public VacancyPostingController(IVacancyPostingMediator vacancyPostingMediator)
        {
            _vacancyPostingMediator = vacancyPostingMediator;
        }

        [HttpGet]
        public ActionResult SelectEmployer(string providerSiteErn)
        {
            var viewModel = new EmployerFilterViewModel
            {
                ProviderSiteErn = providerSiteErn
            };

            var response = _vacancyPostingMediator.GetProviderEmployers(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }

        }

        [HttpPost]
        public ActionResult SelectEmployer(EmployerFilterViewModel viewModel)
        {
            var response = _vacancyPostingMediator.GetProviderEmployers(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }

        }

        [HttpGet]
        public ActionResult AddEmployer(EmployerSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.GetEmployers(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetEmployers.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ConfirmEmployer(string providerSiteErn, string ern)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteErn, ern);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetEmployer.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        public ActionResult ConfirmEmployer(string providerSiteErn, string ern, string description)
        {
            var response = _vacancyPostingMediator.ConfirmEmployer(providerSiteErn, ern, description);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn, ern });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult CreateVacancy(string providerSiteErn, string ern)
        {
            var response = _vacancyPostingMediator.GetNewVacancyModel(providerSiteErn, ern);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult CreateVacancy(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.CreateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CreateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.CreateVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.SubmitVacancy, new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult SubmitVacancy(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SubmitVacancy(VacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SubmitVacancy(viewModel);
            var vacancyViewModel = response.ViewModel;

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.SubmitVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(vacancyViewModel);

                case VacancyPostingMediatorCodes.SubmitVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancySubmitted, new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult VacancySubmitted(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetSubmittedVacancyViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }
    }
}