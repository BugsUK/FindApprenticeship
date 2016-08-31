namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Application.Interfaces;
    using Common.Constants;
    using Common.Framework;
    using FluentValidation.Mvc;
    using Common.Mediators;
    using Mediators.Home;
    using Raa.Common.Constants.Pages;
    using ViewModels.Home;
    using SFA.Infrastructure.Interfaces;

    public class HomeController : RecruitmentControllerBase
    {
        private readonly IHomeMediator _homeMediator;

        public HomeController(IHomeMediator homeMediator, IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
            _homeMediator = homeMediator;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }
                
        public ActionResult ContactUs()
        {
            var userName = GetProviderUserName();
            var response = _homeMediator.GetContactMessageViewModel(userName);
            return View(response.ViewModel);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactMessageViewModel viewModel)
        {
            var userName = GetProviderUserName();
            var response = _homeMediator.SendContactMessage(userName,viewModel);
            switch (response.Code)
            {
                case HomeMediatorCodes.SendContactMessage.ValidationError:
                    ModelState.Clear();
                    response.ValidationResult.AddToModelState(ModelState, string.Empty);
                    return View(viewModel);
                case HomeMediatorCodes.SendContactMessage.SuccessfullySent:
                    ModelState.Clear();
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View(response.ViewModel);
                case HomeMediatorCodes.SendContactMessage.Error:
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View(response.ViewModel);
            }

            throw new InvalidMediatorCodeException(response.Code);
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        public ActionResult Cookies(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl.IsValidReturnUrl() ? returnUrl : "/";
            return View();
        }

        [HttpGet]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        public ActionResult WebTrendsOptOut()
        {

            var webTrendsOptOutCookie = new HttpCookie("WTLOPTOUT", "yes") { Expires = DateTime.UtcNow.AddYears(5) };
            HttpContext.Response.Cookies.Add(webTrendsOptOutCookie);
            SetUserMessage(PrivacyPageMessages.WebTrendsOptOutSuccessful);
            return View("Privacy");
        }
    }
}