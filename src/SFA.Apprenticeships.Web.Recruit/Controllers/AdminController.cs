namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Extensions;
    using Common.Mediators;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Raa;
    using FluentValidation.Mvc;
    using Raa.Common.Mediators.Admin;
    using Raa.Common.ViewModels.Admin;
    using Raa.Common.ViewModels.Provider;
    using System.Security.Claims;
    using System.Web.Mvc;

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
                ModelState.Clear();

                switch (response.Code)
                {
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
        public ActionResult ChooseProvider(TransferVacanciesResultsViewModel vacanciesToBeTransferredVm)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Providers(ProviderSearchViewModel viewModel)
        {
            var response = _adminMediator.SearchProviders(viewModel);

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
        public ActionResult Provider(int providerId)
        {
            var response = _adminMediator.GetProvider(providerId);

            return View(response.ViewModel);
        }

        [HttpGet]
        public ActionResult ManageVacanciesTransfers(int providerId, int providerSiteId)
        {
            var manageVacancyViewModel = new ManageVacancyTransferViewModel
            {
                ProviderId = providerId,
                ProviderSiteId = providerSiteId
            };
            return View(manageVacancyViewModel);
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
            return RedirectToRoute(RecruitmentRouteNames.AdminProviders, viewModel.SearchViewModel);
        }
        public ActionResult ChooseProvider(ProviderSearchResultsViewModel resultsViewModel)
        {
            return View("ChooseProvider");
        }

        //[HttpPost]
        //public ActionResult ConfirmVacancies(TransferVacanciesResultsViewModel resultsViewModel)
        //{
        //    return View("ChooseProvider");
        //}

        //[HttpPost]
        //[MultipleFormActionsButton(SubmitButtonActionName = "ChooseProviderAction")]
        //public ActionResult ChooseProvider(TransferVacanciesResultsViewModel resultsViewModel, TransferVacancyViewModel viewModel)
        //{
        //    return View();
        //}



    }
}