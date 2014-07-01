﻿namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Web.Mvc;
    using Common.Controllers;
    using Infrastructure.Azure.Session;
    using Providers;
    using Validators;
    using ViewModels.Applications;

    public class ApplicationController : SfaControllerBase
    {
        private const string TempAppFormSessionId = "TempAppForm";

        private readonly IApplicationProvider _applicationProvider;
        private readonly AboutYouViewModelValidator _validator;

        public ApplicationController(ISessionState sessionState, 
                                    IApplicationProvider applicationProvider,
                                    AboutYouViewModelValidator validator) : base(sessionState)
        {
            _applicationProvider = applicationProvider;
            _validator = validator;
        }

        public ActionResult Index(int id)
        {
            return View(id);
        }

        public ActionResult Apply(int id, int profileId)
        {
            var applicationViewModel = _applicationProvider.GetApplicationViewModel(id, profileId);

            if (applicationViewModel == null)
            {
                Response.StatusCode = 404;
                return View("VacancyNotFound");
            }

            return View(applicationViewModel);
        }

        [HttpPost]
        public ActionResult Apply(ApplicationViewModel applicationViewModel)
        {
            var result = _validator.Validate(applicationViewModel.Candidate.AboutYou);

            if (!result.IsValid)
            {
                return View(applicationViewModel);
            }

            Session.Store(TempAppFormSessionId, applicationViewModel);
            return RedirectToAction("Preview", new { id = applicationViewModel.VacancyId });
        }

        public ActionResult Preview(int id)
        {
            var appForm = Session.Get<ApplicationViewModel>(TempAppFormSessionId);
            return View(appForm);
        }
    }
}