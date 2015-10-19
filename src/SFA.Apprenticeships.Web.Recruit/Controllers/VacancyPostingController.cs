namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Controllers;
    using Common.Extensions;
    using Common.Mediators;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators.VacancyPosting;
    using Providers;
    using ViewModels.Provider;
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
            var response = _vacancyPostingMediator.GetProviderEmployers(providerSiteErn);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
        
        //[HttpPost]
        //[MultipleFormActionsButton(SubmitButtonActionName = "SearchExistingEmployerByReferenceNumber")]
        //public ActionResult SearchExistingEmployerByReferenceNumber(EmployerSearchViewModel viewModel)
        //{
        //    viewModel.FilterType = EmployerFilterType.Ern;
        //    return RedirectToRoute(RecruitmentRouteNames.SearchExistingEmployer, viewModel);
        //}

        //[HttpPost]
        //[MultipleFormActionsButton(SubmitButtonActionName = "SearchExistingEmployerByNameAndOrLocation")]
        //public ActionResult SearchExistingEmployerByNameAndOrLocation(EmployerSearchViewModel viewModel)
        //{
        //    viewModel.FilterType = EmployerFilterType.NameAndLocation;
        //    return RedirectToRoute(RecruitmentRouteNames.SearchExistingEmployer, viewModel);
        //}

        public ActionResult SearchExistingEmployer(EmployerSearchViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.Ern))
            {
                viewModel.FilterType = EmployerFilterType.Ern;
            }
            else if (!string.IsNullOrWhiteSpace(viewModel.Location) || !string.IsNullOrWhiteSpace(viewModel.Name))
            {
                viewModel.FilterType = EmployerFilterType.NameAndLocation;
            }
            else
            {
                viewModel.FilterType = EmployerFilterType.Undefined;
            }

            var response = _vacancyPostingMediator.GetProviderEmployers(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
                    return View("SelectEmployer", response.ViewModel);
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
        public ActionResult ConfirmEmployer(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var response = _vacancyPostingMediator.ConfirmEmployer(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult CreateVacancy(string providerSiteErn, string ern)
        {
            var response = _vacancyPostingMediator.GetNewVacancyViewModel(User.GetUkprn(), providerSiteErn, ern);
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
                    return RedirectToRoute(RecruitmentRouteNames.VacancySummary, new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult VacancySummary(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancySummaryViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult VacancySummary(VacancySummaryViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyRequirementsProspects, new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult VacancyRequirementsProspects(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyRequirementsProspectsViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult VacancyRequirementsProspects(VacancyRequirementsProspectsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.VacancyQuestions, new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult VacancyQuestions(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyQuestionsViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult VacancyQuestions(VacancyQuestionsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy, new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    });

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult PreviewVacancy(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancyViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
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

        [HttpGet]
        public ActionResult SelectNewEmployer(EmployerSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.SelectNewEmployer(viewModel);

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.SelectNewEmployer.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.SelectNewEmployer.NoResults:
                case VacancyPostingMediatorCodes.SelectNewEmployer.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SelectNewEmployerByReferenceNumber")]
        public ActionResult SelectNewEmployerByReferenceNumber(EmployerSearchViewModel viewModel)
        {
            viewModel.FilterType = EmployerFilterType.Ern;
            return RedirectToRoute(RecruitmentRouteNames.SelectNewEmployer, viewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SelectNewEmployerByNameAndLocation")]
        public ActionResult SelectNewEmployerByNameAndLocation(EmployerSearchViewModel viewModel)
        {
            viewModel.FilterType = EmployerFilterType.NameAndLocation;
            return RedirectToRoute(RecruitmentRouteNames.SelectNewEmployer, viewModel);
        }

        [HttpGet]
        public ActionResult ConfirmNewEmployer(string providerSiteErn, string ern)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteErn, ern);
            return View(response.ViewModel);
        }

        [HttpPost]
        public ActionResult ConfirmNewEmployer(ProviderSiteEmployerLinkViewModel viewModel)
        {
            var response = _vacancyPostingMediator.ConfirmEmployer(viewModel);

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.ConfirmEmployer.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}