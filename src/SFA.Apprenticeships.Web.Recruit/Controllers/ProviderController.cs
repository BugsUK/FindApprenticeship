namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Extensions;
    using Common.Mediators;
    using FluentValidation.Mvc;
    using Mediators.Provider;
    using Raa.Common.ViewModels.Provider;
    using Constants;
    using Domain.Entities;
    using Domain.Entities.Raa;

    [AuthorizeUser(Roles = Roles.Faa)]
    [OwinSessionTimeout]
    public class ProviderController : RecruitmentControllerBase
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
            var response = _providerMediator.UpdateSites(User.GetUkprn(), User.Identity.Name, providerViewModel);

            ModelState.Clear();

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
        }

        [HttpGet]
        [AuthorizeUser(Roles = Roles.VerifiedEmail)]
        public ActionResult AddSite()
        {
            var response = _providerMediator.AddSite();

            return View(response.ViewModel);
        }

        [HttpPost]
        [AuthorizeUser(Roles = Roles.VerifiedEmail)]
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
        [AuthorizeUser(Roles = Roles.VerifiedEmail)]
        public ActionResult EditSite(string edsUrn)
        {
            var response = _providerMediator.GetSite(User.GetUkprn(), edsUrn);
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
                    return RedirectToRoute(RecruitmentRouteNames.EditProviderSite, new { providerSiteId = response.ViewModel.ProviderSiteId });
                default:
                    throw new InvalidMediatorCodeException(response.Code);
            }
        }
    }
}