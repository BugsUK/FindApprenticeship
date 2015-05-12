namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Configuration;
    using Common.Constants;
    using Constants;
    using Constants.Pages;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Register;
    using Providers;
    using ViewModels.Candidate;
    using ViewModels.Register;

    public class RegisterController : CandidateControllerBase
    {
        private readonly IRegisterMediator _registerMediator;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly IAccountProvider _accountProvider;

        public RegisterController(ICandidateServiceProvider candidateServiceProvider,
            IAccountProvider accountProvider,
            IRegisterMediator registerMediator,
            IConfigurationService configurationService) 
            : base(configurationService)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _accountProvider = accountProvider;
            _registerMediator = registerMediator;
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [AllowReturnUrl(Allow = false)]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() => View(new RegisterViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(RegisterViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _registerMediator.Register(model);

                ModelState.Clear();

                switch (response.Code)
                {
                    case RegisterMediatorCodes.Register.ValidationFailed:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case RegisterMediatorCodes.Register.RegistrationFailed:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(model);
                    case RegisterMediatorCodes.Register.SuccessfullyRegistered:
                        UserData.SetUserContext(model.EmailAddress, model.Firstname + " " + model.Lastname, ConfigurationService.Get<WebConfiguration>().TermsAndConditionsVersion);
                        return RedirectToAction("Activation");
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }
            });
        }

        [HttpGet]
        [AuthorizeCandidate(Roles = UserRoleNames.Unactivated)]
        [AllowReturnUrl(Allow = false)]
        public async Task<ActionResult> Activation(string returnUrl)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var model = new ActivationViewModel { EmailAddress = UserContext.UserName };

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    UserData.Push(UserDataItemNames.ReturnUrl, Server.UrlEncode(returnUrl));
                }

                return View(model);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCandidate(Roles = UserRoleNames.Unactivated)]
        public async Task<ActionResult> Activate(ActivationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _registerMediator.Activate(UserContext.CandidateId, model);

                switch (response.Code)
                {
                    case RegisterMediatorCodes.Activate.SuccessfullyActivated:
                        SetUserMessage(response.Message.Text);
                        var candidate = _candidateServiceProvider.GetCandidate(model.EmailAddress);
                        UserData.SetUserContext(candidate.RegistrationDetails.EmailAddress,
                            candidate.RegistrationDetails.FirstName + " " + candidate.RegistrationDetails.LastName,
                            candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion);
                        return RedirectToRoute(RouteNames.MonitoringInformation);
                    case RegisterMediatorCodes.Activate.InvalidActivationCode:
                    case RegisterMediatorCodes.Activate.FailedValidation:
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        break;
                    case RegisterMediatorCodes.Activate.ErrorActivating:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        break;
                    default:
                        throw new InvalidMediatorCodeException(response.Code);
                }

                return View("Activation", model);
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> MonitoringInformation()
        {
            return await Task.Run(() =>
            {
                var settingsViewModel = _accountProvider.GetSettingsViewModel(UserContext.CandidateId);
                return View("MonitoringInformation", settingsViewModel.MonitoringInformation);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> MonitoringInformation(MonitoringInformationViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _registerMediator.UpdateMonitoringInformation(UserContext.CandidateId, model);

                switch (response.Code)
                {
                    case RegisterMediatorCodes.UpdateMonitoringInformation.FailedValidation:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                }

                //Redirects even if fails for unknown reason, don't hinder user.
                return RedirectToAction("SkipMonitoringInformation");
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> SkipMonitoringInformation()
        {
            return await Task.Run<ActionResult>(() =>
            {
                // ReturnUrl takes precedence over last view vacnacy id.
                var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

                // Clear last viewed vacancy and distance (if any).
                var lastViewedVacancyId = UserData.Pop(CandidateDataItemNames.LastViewedVacancyId);
                UserData.Pop(CandidateDataItemNames.VacancyDistance);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(Server.UrlDecode(returnUrl));
                }

                if (lastViewedVacancyId != null)
                {
                    return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id = int.Parse(lastViewedVacancyId) });
                }

                return RedirectToRoute(CandidateRouteNames.ApprenticeshipSearch);
            });
        }

        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> Complete()
        {
            return await Task.Run(() => View(UserContext.UserName));
        }

        [AllowReturnUrl(Allow = false)]
        public async Task<ActionResult> ResendActivationCode(string emailAddress)
        {
            return await Task.Run(() =>
            {
                if (_candidateServiceProvider.ResendActivationCode(emailAddress))
                {
                    SetUserMessage(string.Format(ActivationPageMessages.ActivationCodeSent, emailAddress));

                    return RedirectToAction("Activation");
                }

                SetUserMessage(ActivationPageMessages.ActivationCodeSendingFailure, UserMessageLevel.Warning);
                return RedirectToAction("Activation");
            });
        }

        [AllowCrossSiteJson]
        public async Task<ActionResult> CheckUsername(string username)
        {
            return await Task.Run(() =>
            {
                var userNameAvailability = _candidateServiceProvider.IsUsernameAvailable(username.Trim());

                return Json(userNameAvailability, JsonRequestBehavior.AllowGet);
            });
        }
    }
}
