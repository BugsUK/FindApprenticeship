namespace SFA.Apprenticeships.Web.ContactForms.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Constants;
    using Constants.Pages;
    using Framework.Attributes;
    using Mediators;
    using Mediators.Interfaces;
    using ViewModels;
    using FluentValidation.Mvc;
    using Mediators.AccessRequest;

    public class AccessRequestController : ContactFormsControllerBase
    {
        private readonly IAccessRequestMediator _accessRequestMediator;

        public AccessRequestController(IAccessRequestMediator accessRequestMediator)
        {
            _accessRequestMediator = accessRequestMediator;
        }

        [HttpGet]
        public async Task<ActionResult> SubmitAccessRequest()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var result = _accessRequestMediator.SubmitAccessRequest();
                return View(result.ViewModel);
            });
        }

        [HttpPost]
        [HoneypotCaptcha("UserName")]
        public async Task<ActionResult> SubmitAccessRequest(AccessRequestViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _accessRequestMediator.SubmitAccessRequest(model);
                ModelState.Clear();

                switch (response.Code)
                {
                    case AccessRequestMediatorCodes.SubmitAccessRequest.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case AccessRequestMediatorCodes.SubmitAccessRequest.Error:
                        SetPageMessage(response.Message.Text, response.Message.Level);
                        return View(model);
                    case AccessRequestMediatorCodes.SubmitAccessRequest.Success:
                        return RedirectToRoute(EmployerRouteNames.AccessRequestThankYou);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        public async Task<ActionResult> ThankYou()
        {
            SetPageMessage(AccessRequestPageMessages.RequestHasBeenSubmittedSuccessfully);
            ViewBag.Title = "Access request-Thank You";
            return await Task.Run<ActionResult>(() => View());
        }
    }
}