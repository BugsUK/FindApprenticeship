using SFA.Apprenticeships.Web.Recruit.Constants;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Common.Attributes;
    using Common.Controllers;
    using Common.Mediators;
    using Extensions;
    using FluentValidation.Mvc;
    using Mediators.Provider;
    using Providers;
    using ViewModels.Provider;

    [Authorize(Roles = Roles.Faa)]
    public class ProviderController : ControllerBase<RecuitmentUserContext>
    {
        private readonly IProviderMediator _providerMediator;

        public ProviderController(
            IProviderMediator providerMediator)
        {
            _providerMediator = providerMediator;
        }

        [HttpGet]
        public ActionResult Sites()
        {
            var ukprn = User.GetUkprn();
            var response = _providerMediator.Sites(ukprn);
            var providerProfile = response.ViewModel;

            return View(providerProfile);
        }

        [HttpPost]
        public ActionResult Sites(ProviderViewModel providerViewModel)
        {
            var response = _providerMediator.UpdateSites(providerViewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case ProviderMediatorCodes.UpdateSites.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case ProviderMediatorCodes.UpdateSites.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ManageProviderSites);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }

        [HttpGet]
        [Authorize(Roles = Roles.VerifiedEmail)]
        public ActionResult AddSite()
        {
            var response = _providerMediator.AddSite();

            return View(response.ViewModel);
        }

        [HttpPost]
        [Authorize(Roles = Roles.VerifiedEmail)]
        [MultipleFormActionsButton(SubmitButtonActionName = "AddSiteByEmployerReferenceNumber")]
        public ActionResult AddSiteByEmployerReferenceNumber(ProviderSiteSearchViewModel viewModel)
        {
            var response = _providerMediator.AddSite(viewModel);

            ModelState.Clear();

            switch (response.Code)
            {
                case ProviderMediatorCodes.AddSite.ValidationError:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View("AddSite", response.ViewModel);

                case ProviderMediatorCodes.AddSite.SiteNotFoundByEmployerReferenceNumber:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View("AddSite", response.ViewModel);

                case ProviderMediatorCodes.AddSite.SiteFoundByEmployerReferenceNumber:
                    // TODO: AG: return RedirectToRoute(Xxx);
                    break;

                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }

            return View("AddSite", response.ViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Roles.VerifiedEmail)]
        public ActionResult EditSite(string ern)
        {
            var response = _providerMediator.GetSite(ern);
            var providerSiteViewModel = response.ViewModel;

            return View(providerSiteViewModel);
        }

        [HttpPost]
        [Authorize(Roles = Roles.VerifiedEmail)]
        public ActionResult EditSite(ProviderSiteViewModel providerSiteViewModel)
        {
            var response = _providerMediator.UpdateSite(providerSiteViewModel);

            switch (response.Code)
            {
                case ProviderMediatorCodes.UpdateSite.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case ProviderMediatorCodes.UpdateSite.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.EditProviderSite, new { ern = response.ViewModel.Ern });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}