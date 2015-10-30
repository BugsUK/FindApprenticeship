using System;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Extensions;
    using Common.Mediators;
    using Common.Validators.Extensions;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators.VacancyPosting;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    [AuthorizeUser(Roles = Roles.Faa)]
    [OwinSessionTimeout]
    public class VacancyPostingController : RecruitmentControllerBase
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

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
                case VacancyPostingMediatorCodes.GetProviderEmployers.NoResults:
                    return View(response.ViewModel);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
        
        [HttpGet]
        public ActionResult SearchExistingEmployer(EmployerSearchViewModel viewModel)
        {
            var response = _vacancyPostingMediator.GetProviderEmployers(viewModel);

            ModelState.Clear();

            if (response.Message != null)
            {
                SetUserMessage(response.Message.Text, response.Message.Level);
            }

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.GetProviderEmployers.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View("SelectEmployer", response.ViewModel);

                case VacancyPostingMediatorCodes.GetProviderEmployers.Ok:
                case VacancyPostingMediatorCodes.GetProviderEmployers.NoResults:
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
        public ActionResult ConfirmEmployer(string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteErn, ern, vacancyGuid);

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
                    ModelState.Clear();
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case VacancyPostingMediatorCodes.ConfirmEmployer.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult CreateVacancy(string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var response = _vacancyPostingMediator.GetNewVacancyViewModel(User.GetUkprn(), providerSiteErn, ern, vacancyGuid);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CreateVacancy")]
        [HttpPost]
        public ActionResult CreateVacancyAndExit(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.CreateVacancyAndExit(viewModel);

            Func<ActionResult> okAction = () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

            return HandleCreateVacancy(response, okAction);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "CreateVacancy")]
        [HttpPost]
        public ActionResult CreateVacancy(NewVacancyViewModel viewModel)
        {
            var response = _vacancyPostingMediator.CreateVacancy(viewModel);

            Func<ActionResult> okAction = () => RedirectToRoute(RecruitmentRouteNames.VacancySummary, 
                new
                {
                    vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                });

            return HandleCreateVacancy(response, okAction);
        }

        private ActionResult HandleCreateVacancy(MediatorResponse<NewVacancyViewModel> response, Func<ActionResult> okAction )
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.CreateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.CreateVacancy.Ok:
                    return okAction();

                case VacancyPostingMediatorCodes.CreateVacancy.OkWithWarning:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return okAction();

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult EditVacancy(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetNewVacancyViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View("CreateVacancy", viewModel);
        }

        [HttpGet]
        public ActionResult VacancySummary(long vacancyReferenceNumber)
        {
            var response = _vacancyPostingMediator.GetVacancySummaryViewModel(vacancyReferenceNumber);
            var viewModel = response.ViewModel;

            return View(viewModel);
        }


        [MultipleFormActionsButton(SubmitButtonActionName = "VacancySummary")]
        [HttpPost]
        public ActionResult VacancySummary(VacancySummaryViewModel viewModel, bool acceptWarnings)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel, acceptWarnings);

            return HandleVacancySummary(response,
                () => RedirectToRoute(RecruitmentRouteNames.VacancyRequirementsProspects,
                    new
                    {
                        vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                    }));
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancySummary")]
        [HttpPost]
        public ActionResult VacancySummaryAndExit(VacancySummaryViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancyAndExit(viewModel);

            return HandleVacancySummary(response, () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome));
        }

        private ActionResult HandleVacancySummary(MediatorResponse<VacancySummaryViewModel> response,
            Func<ActionResult> okAction)
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelStateWithSeverity(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return okAction();

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

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyRequirementsProspects")]
        [HttpPost]
        public ActionResult VacancyRequirementsProspects(VacancyRequirementsProspectsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            return HandleVacancyRequirementsProspects(response);
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyRequirementsProspects")]
        [HttpPost]
        public ActionResult VacancyRequirementsProspectsAndExit(VacancyRequirementsProspectsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancyAndExit(viewModel);

            return HandleVacancyRequirementsProspects(response);
        }

        private ActionResult HandleVacancyRequirementsProspects(
            MediatorResponse<VacancyRequirementsProspectsViewModel> response)
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.OkAndExit:
                    return RedirectToRoute(RecruitmentRouteNames.RecruitmentHome);

                case VacancyPostingMediatorCodes.UpdateVacancy.OfflineVacancyOk:
                    return RedirectToRoute(RecruitmentRouteNames.PreviewVacancy, new
                        {
                            vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                        });

                case VacancyPostingMediatorCodes.UpdateVacancy.OnlineVacancyOk:
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


        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQuestions")]
        [HttpPost]
        public ActionResult VacancyQuestions(VacancyQuestionsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancy(viewModel);

            return HandleVacancyQuestions(response, () => RedirectToRoute(RecruitmentRouteNames.PreviewVacancy,
                new
                {
                    vacancyReferenceNumber = response.ViewModel.VacancyReferenceNumber
                }));
        }

        [MultipleFormActionsButton(SubmitButtonActionName = "VacancyQuestions")]
        [HttpPost]
        public ActionResult VacancyQuestionsAndExit(VacancyQuestionsViewModel viewModel)
        {
            var response = _vacancyPostingMediator.UpdateVacancyAndExit(viewModel);

            return HandleVacancyQuestions(response, () => RedirectToRoute(RecruitmentRouteNames.RecruitmentHome));
        }

        private ActionResult HandleVacancyQuestions(MediatorResponse<VacancyQuestionsViewModel> response,
            Func<ActionResult> okAction)
        {
            ModelState.Clear();

            switch (response.Code)
            {
                case VacancyPostingMediatorCodes.UpdateVacancy.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);

                case VacancyPostingMediatorCodes.UpdateVacancy.Ok:
                    return okAction();

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

        [HttpGet]
        public ActionResult ConfirmNewEmployer(string providerSiteErn, string ern, Guid vacancyGuid)
        {
            var response = _vacancyPostingMediator.GetEmployer(providerSiteErn, ern, vacancyGuid);
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
                    return RedirectToRoute(RecruitmentRouteNames.CreateVacancy, new { providerSiteErn = response.ViewModel.ProviderSiteErn, ern = response.ViewModel.Employer.Ern, vacancyGuid = response.ViewModel.VacancyGuid });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}