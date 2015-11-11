using SFA.Apprenticeships.Web.Recruit.Constants;

namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using ViewModels.Provider;

    public class ProviderController : Common.Controllers.ControllerBase
    {
        [HttpGet]
        public ActionResult Sites()
        {
            var model = new ProviderViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Sites(ProviderViewModel providerViewModel)
        {
            /*
            switch (response.Code)
            {
                case ProviderMediatorCodes.UpdateSites.FailedValidation:
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(response.ViewModel);
                case ProviderMediatorCodes.UpdateSites.NoUserProfile:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return RedirectToRoute(RecruitmentRouteNames.Settings);
                case ProviderMediatorCodes.UpdateSites.Ok:
                    return RedirectToRoute(RecruitmentRouteNames.ManageProviderSites);
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
            */

            return RedirectToRoute(RecruitmentRouteNames.ManageProviderSites);
        }

        /*
        [HttpGet]
        public ActionResult AddSite()
        {
            var response = _providerMediator.AddSite();

            return View(response.ViewModel);
        }

        [HttpPost]
        //[MultipleFormActionsButton(SubmitButtonActionName = "AddSiteByEmployerReferenceNumber")]
        public ActionResult AddSiteByEmployerReferenceNumber(ProviderSiteSearchViewModel viewModel)
        {
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
        [AuthorizeUser(Roles = Roles.VerifiedEmail)]
        public ActionResult EditSite(string ern)
        {
            var response = _providerMediator.GetSite(User.GetUkprn(), ern);
            var providerSiteViewModel = response.ViewModel;

            return View(providerSiteViewModel);
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.VerifiedEmail)]
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
    */
    }
}