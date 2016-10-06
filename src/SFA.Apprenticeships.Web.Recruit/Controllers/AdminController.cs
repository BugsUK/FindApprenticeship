namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Extensions;
    using Common.Mediators;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Raa;
    using FluentValidation.Mvc;
    using Raa.Common.Mediators.Admin;
    using Raa.Common.ViewModels.Admin;
    using Raa.Common.ViewModels.Provider;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Mvc;
    using Raa.Common.ViewModels.ProviderUser;

    [AuthorizeUser(Roles = Roles.Faa)]
    [AuthorizeUser(Roles = Roles.Admin)]
    public class AdminController : RecruitmentControllerBase
    {
        private readonly ICookieAuthorizationDataProvider _cookieAuthorizationDataProvider;
        private readonly IAdminMediator _adminMediator;

        public AdminController(ICookieAuthorizationDataProvider cookieAuthorizationDataProvider,
            IConfigurationService configurationService, ILogService loggingService, IAdminMediator adminMediator)
            : base(configurationService, loggingService)
        {
            _cookieAuthorizationDataProvider = cookieAuthorizationDataProvider;
            _adminMediator = adminMediator;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProviderUsers(ProviderUserSearchViewModel viewModel)
        {
            var response = _adminMediator.SearchProviderUsers(viewModel, User.GetUkprn());

            ModelState.Clear();

            switch (response.Code)
            {
                case AdminMediatorCodes.SearchProviderUsers.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View(response.ViewModel);

                case AdminMediatorCodes.SearchProviderUsers.Ok:
                    return View(response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ProviderUser(int providerUserId)
        {
            var response = _adminMediator.GetProviderUser(providerUserId);

            return View(response.ViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SearchProviderUsersAction")]
        public ActionResult SearchProviderUsers(ProviderUserSearchResultsViewModel viewModel)
        {
            viewModel.SearchViewModel.PerformSearch = true;
            return RedirectToRoute(RecruitmentRouteNames.AdminProviderUsers, viewModel.SearchViewModel);
        }

        [HttpGet]
        public ActionResult ChangeUkprn()
        {
            return View(new ChangeUkprnViewModel { Ukprn = User.GetUkprn() });
        }

        [HttpPost]
        public ActionResult ChangeUkprn(ChangeUkprnViewModel viewModel)
        {
            var claim = new Claim(Common.Constants.ClaimTypes.UkprnOverride, viewModel.Ukprn);

            RemoveUkprnOverride();
            _cookieAuthorizationDataProvider.AddClaim(claim, HttpContext, User.Identity.Name);

            SetUserMessage($"Your UKPRN has been changed to {viewModel.Ukprn}");

            return RedirectToRoute(RecruitmentRouteNames.AdminChangeUkprn);
        }

        [HttpGet]
        public ActionResult ResetUkprn()
        {
            RemoveUkprnOverride();

            SetUserMessage("Your UKPRN has been reset");

            return RedirectToRoute(RecruitmentRouteNames.AdminChangeUkprn);
        }

        private void RemoveUkprnOverride()
        {
            _cookieAuthorizationDataProvider.RemoveClaim(Common.Constants.ClaimTypes.UkprnOverride, User.GetUkprn(), HttpContext,
                User.Identity.Name);
        }

        public ActionResult TransferVacancies()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransferVacancies(TransferVacanciesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = _adminMediator.GetVacancyDetails(viewModel);
                if (response.ViewModel.NotFoundVacancyNumbers.Any())
                {
                    SetUserMessage("No vacancies found for the given vacancy reference numbers: " +
                                   $"{string.Join(", ", response.ViewModel.NotFoundVacancyNumbers)}", UserMessageLevel.Error);
                }
                ModelState.Clear();

                switch (response.Code)
                {
                    case AdminMediatorCodes.GetVacancyDetails.NoRecordsFound:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View();
                    case AdminMediatorCodes.GetVacancyDetails.Ok:
                        return View("ConfirmVacancies", response.ViewModel);
                    case AdminMediatorCodes.GetVacancyDetails.FailedAuthorisation:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View();
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            }
            return View();
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "ChooseProviderAction")]
        public ActionResult ChooseProvider(List<int> vacancyreferencenumber)
        {
            ProviderSearchResultsViewModel providerSearchResultsViewModel = new ProviderSearchResultsViewModel()
            {
                VacancyReferenceNumbers = vacancyreferencenumber
            };
            return View(providerSearchResultsViewModel);
        }

        [HttpGet]
        public ActionResult Providers(ProviderSearchViewModel viewModel)
        {
            var response = _adminMediator.SearchProviders(viewModel);
            response.ViewModel.VacancyReferenceNumbers = viewModel.VacancyReferenceNumbers.Split(',').Select(int.Parse).ToList();
            ModelState.Clear();

            switch (response.Code)
            {
                case AdminMediatorCodes.SearchProviders.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, "SearchViewModel");
                    return View("ChooseProvider", response.ViewModel);

                case AdminMediatorCodes.SearchProviders.Ok:
                    return View("ChooseProvider", response.ViewModel);

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult Provider(int providerId, string vacancyreferencenumbers)
        {
            var response = _adminMediator.GetProvider(providerId);
            response.ViewModel.VacanciesReferenceNumbers = vacancyreferencenumbers;
            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult ManageVacanciesTransfers(int providerId, int providerSiteId, string vacanciesReferenceNumbers)
        {
            var providerResponse = _adminMediator.GetProvider(providerId);
            var providerSiteResponse = _adminMediator.GetProviderSite(providerSiteId);
            var manageVacancyViewModel = new ManageVacancyTransferViewModel
            {
                ProviderId = providerId,
                ProviderName = providerResponse.ViewModel.FullName,
                ProviderSiteId = providerSiteId,
                ProviderSiteName = providerSiteResponse.ViewModel.DisplayName + " (" + providerSiteResponse.ViewModel.EdsUrn + ")"
            };
            if (vacanciesReferenceNumbers != null)
                manageVacancyViewModel.VacancyReferenceNumbers =
                    vacanciesReferenceNumbers.Split(',').Select(int.Parse).ToList();
            return View(manageVacancyViewModel);
        }

        [HttpPost]
        public ActionResult ManageVacanciesTransfers(ManageVacancyTransferViewModel vacancyTransferViewModel)
        {
            var response = _adminMediator.ManageVacanciesTransfers(vacancyTransferViewModel);
            ModelState.Clear();

            switch (response.Code)
            {
                case AdminMediatorCodes.TransferVacancy.Ok:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View("TransferVacancies");
                case AdminMediatorCodes.TransferVacancy.FailedTransfer:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View("TransferVacancies");
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        public ActionResult ProviderSite(int providerSiteId)
        {
            var response = _adminMediator.GetProviderSite(providerSiteId);

            return View(response.ViewModel);
        }

        [HttpPost]
        [MultipleFormActionsButton(SubmitButtonActionName = "SearchProvidersAction")]
        public ActionResult SearchProviders(ProviderSearchResultsViewModel viewModel)
        {
            viewModel.SearchViewModel.PerformSearch = true;
            if (viewModel.VacancyReferenceNumbers != null)
            {
                viewModel.SearchViewModel.VacancyReferenceNumbers = string.Join(",", viewModel.VacancyReferenceNumbers);
            }
            return RedirectToRoute(RecruitmentRouteNames.AdminProviders, viewModel.SearchViewModel);
        }

        public ActionResult ChooseProvider(ProviderSearchResultsViewModel resultsViewModel)
        {
            return View("ChooseProvider");
        }
    }
}