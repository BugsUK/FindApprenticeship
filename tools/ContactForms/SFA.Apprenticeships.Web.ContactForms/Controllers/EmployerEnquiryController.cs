namespace SFA.Apprenticeships.Web.ContactForms.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Constants;
    using Constants.Pages;
    using FluentValidation.Mvc;
    using Framework.Attributes;
    using Mediators;
    using Mediators.EmployerEnquiry;
    using Mediators.Interfaces;
    using ViewModels;

    public class EmployerEnquiryController : ContactFormsControllerBase
    {
        private readonly IEmployerEnquiryMediator _employerEnquiryMediator;

        public EmployerEnquiryController(IEmployerEnquiryMediator employerEnquiryMediator)
        {
            _employerEnquiryMediator = employerEnquiryMediator;
        }

        [HttpGet]
        public async Task<ActionResult> SubmitEmployerEnquiry()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var result = _employerEnquiryMediator.SubmitEnquiry();
                return View(result.ViewModel);
            });
        }
        
        [HttpPost]
        [HoneypotCaptcha("UserName")]
        public async Task<ActionResult> SubmitEmployerEnquiry(EmployerEnquiryViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _employerEnquiryMediator.SubmitEnquiry(model);
                ModelState.Clear();

                switch (response.Code)
                {
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.Error:
                        SetPageMessage(response.Message.Text, response.Message.Level);
                        return View(model);
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.Success:
                        return RedirectToRoute(EmployerRouteNames.SubmitEmployerEnquiryThankYou);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }


        [HttpGet]
        public async Task<ActionResult> GlaSubmitEmployerEnquiry()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var result = _employerEnquiryMediator.SubmitGlaEnquiry();
                return View(result.ViewModel);
            });
        }

        [HttpPost]
        [HoneypotCaptcha("UserName")]
        public async Task<ActionResult> GlaSubmitEmployerEnquiry(EmployerEnquiryViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _employerEnquiryMediator.SubmitGlaEnquiry(model);
                ModelState.Clear();

                switch (response.Code)
                {
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.ValidationError:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.Error:
                        SetPageMessage(response.Message.Text, response.Message.Level);
                        return View(model);
                    case EmployerEnquiryMediatorCodes.SubmitEnquiry.Success:
                        return RedirectToRoute(EmployerRouteNames.GlaSubmitEmployerEnquiryThankYou);
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        public async Task<ActionResult> ThankYou()
        {
            SetPageMessage(EmployerEnquiryPageMessages.QueryHasBeenSubmittedSuccessfully);
            ViewBag.Title = "Employer enquiry-Thank You";
            return await Task.Run<ActionResult>(() => View());
        }

        public async Task<ActionResult> GlaThankYou()
        {
            SetPageMessage(EmployerEnquiryPageMessages.QueryHasBeenSubmittedSuccessfully);
            ViewBag.Title = "Gla Employer enquiry-Thank You";
            return await Task.Run<ActionResult>(() => View());
        }
    }
}