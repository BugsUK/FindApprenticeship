using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Framework;
    using Constants.Pages;
    using SFA.Infrastructure.Interfaces;
    using FluentValidation.Mvc;
    using Mediators.Home;
    using ViewModels.Home;

    public class HomeController : CandidateControllerBase
    {
        private readonly IHomeMediator _homeMediator;

        public HomeController(IHomeMediator homeMediator, 
            IConfigurationService configurationService,
            ILogService logService)
            : base(configurationService, logService)
        {
            _homeMediator = homeMediator;
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [SiteRootRedirect]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public async Task<ActionResult> Privacy()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        public async Task<ActionResult> WebTrendsOptOut()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var webTrendsOptOutCookie = new HttpCookie("WTLOPTOUT", "yes") {Expires = DateTime.UtcNow.AddYears(5)};
                HttpContext.Response.Cookies.Add(webTrendsOptOutCookie);
                SetUserMessage(PrivacyPageMessages.WebTrendsOptOutSuccessful);
                return View("Privacy");
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public async Task<ActionResult> Cookies(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl.IsValidReturnUrl() ? returnUrl : "/";
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [SessionTimeout]
        public async Task<ActionResult> Helpdesk()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();
                var response = _homeMediator.GetContactMessageViewModel(candidateId);

                return View(response.ViewModel);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Helpdesk(ContactMessageViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();
                var response = _homeMediator.SendContactMessage(candidateId, model);

                switch (response.Code)
                {
                    case HomeMediatorCodes.SendContactMessage.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case HomeMediatorCodes.SendContactMessage.SuccessfullySent:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case HomeMediatorCodes.SendContactMessage.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [SessionTimeout]
        public async Task<ActionResult> Feedback()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();
                var response = _homeMediator.GetFeedbackViewModel(candidateId);

                return View(response.ViewModel);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Feedback(FeedbackViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();
                var response = _homeMediator.SendFeedback(candidateId, model);

                switch (response.Code)
                {
                    case HomeMediatorCodes.SendFeedback.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case HomeMediatorCodes.SendFeedback.SuccessfullySent:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case HomeMediatorCodes.SendFeedback.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public async Task<ActionResult> Terms()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public async Task<ActionResult> NextSteps()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public async Task<ActionResult> HowToApply()
        {
            return await Task.Run<ActionResult>(() => View());
        }
    }
}