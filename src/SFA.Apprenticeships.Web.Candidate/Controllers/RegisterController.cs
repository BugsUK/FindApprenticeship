using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Attributes;
    using Common.Attributes;
    using Common.Configuration;
    using Common.Constants;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using Extensions;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Register;
    using Providers;

    using SFA.Apprenticeships.Application.Interfaces;

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
            IConfigurationService configurationService,
            ILogService logService)
            : base(configurationService, logService)
        {
            _candidateServiceProvider = candidateServiceProvider;
            _accountProvider = accountProvider;
            _registerMediator = registerMediator;
        }

        [HttpGet]
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
                        UserData.SetUserContext(model.EmailAddress, model.Firstname + " " + model.Lastname, _configurationService.Get<CommonWebConfiguration>().TermsAndConditionsVersion);
                        return RedirectToRoute(RouteNames.Activation);
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
        [SessionTimeout]
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
                return RedirectToRoute(RouteNames.SkipMonitoringInformation);
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> SkipMonitoringInformation()
        {
            return await Task.Run<ActionResult>(() =>
            {
                // ReturnUrl takes precedence over last view vacancy id.
                var returnUrl = UserData.Pop(UserDataItemNames.ReturnUrl);

                //Following registration, user should be prompted to verify their mobile number
                var response = _registerMediator.SendMobileVerificationCode(UserContext.CandidateId, Url.RouteUrl(CandidateRouteNames.VerifyMobile, new RouteValueDictionary { { "ReturnUrl", returnUrl } }));
                if (response.Code == RegisterMediatorCodes.SendMobileVerificationCode.Success)
                {
                    var message = response.Message;
                    SetUserMessage(message.Text, message.Level);
                }

                // Clear last viewed vacancy and distance (if any).
                var lastViewedVacancy = UserData.PopLastViewedVacancy();
                UserData.Pop(CandidateDataItemNames.VacancyDistance);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(Server.UrlDecode(returnUrl));
                }

                if (lastViewedVacancy != null)
                {
                    switch (lastViewedVacancy.Type)
                    {
                        case VacancyType.Apprenticeship:
                            return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id = lastViewedVacancy.Id });
                        case VacancyType.Traineeship:
                            return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id = lastViewedVacancy.Id });
                    }
                }

                return RedirectToRoute(CandidateRouteNames.ApprenticeshipSearch);
            });
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        [SessionTimeout]
        public async Task<ActionResult> Complete()
        {
            return await Task.Run(() => View(UserContext.UserName));
        }

        [HttpGet]
        [AllowReturnUrl(Allow = false)]
        public async Task<ActionResult> ResendActivationCode(string emailAddress)
        {
            return await Task.Run(() =>
            {
                if (_candidateServiceProvider.ResendActivationCode(emailAddress))
                {
                    SetUserMessage(string.Format(ActivationPageMessages.ActivationCodeSent, emailAddress));

                    return RedirectToRoute(RouteNames.Activation);
                }

                SetUserMessage(ActivationPageMessages.ActivationCodeSendingFailure, UserMessageLevel.Warning);
                return RedirectToRoute(RouteNames.Activation);
            });
        }

        [HttpGet]
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
