namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Configuration;
    using Common.Constants;
    using Common.Controllers;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Logging;
    using NLog.Contrib;
    using Providers;

    [ApplyWebTrends,
    DefaultSessionTimeout,
    CookiesEnabled,
    OutputCache(CacheProfile = CacheProfiles.None),
    AllowReturnUrl(Allow = true),
    ClearSearchReturnUrl,
    PlannedOutageMessage,
    UserJourneyContext(UserJourney.None, Order = 1),
    RobotsIndexPage]
    public abstract class CandidateControllerBase : ControllerBase<CandidateUserContext>
    {
        public readonly IConfigurationService ConfigurationService;

        protected CandidateControllerBase(IConfigurationService configurationService, ILogService loggingService) : base(loggingService)
        {
            ConfigurationService = configurationService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetPersistentLoggingInfo();
            MappedDiagnosticsLogicalContext.Set("userId", UserContext != null ? UserContext.CandidateId.ToString() : "<none>");

            LogOnActionExecuting(filterContext);

            UserContext = null;

            if (!string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                var context = UserData.GetUserContext();

                if (context != null)
                {
                    var candidateContext = new CandidateUserContext
                    {
                        CandidateId = new Guid(User.Identity.Name),
                        FullName = context.FullName,
                        UserName = context.UserName,
                        AcceptedTermsAndConditionsVersion = context.AcceptedTermsAndConditionsVersion
                    };

                    UserContext = candidateContext;
                }
            }

            SetAbout();
            SetCandidate();

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _logService.Info("Request completed");

            base.OnActionExecuted(filterContext);
        }

        private void SetAbout()
        {
            var webConfiguration = ConfigurationService.Get<CommonWebConfiguration>();
            ViewBag.ShowAbout = webConfiguration.ShowAbout;

            if (!webConfiguration.ShowAbout) return;

            ViewBag.Version = VersionLogging.GetVersion();
            ViewBag.Environment = webConfiguration.Environment;
        }



        private void SetCandidate()
        {
            var user = HttpContext.User;

            if (user != null)
            {
                ViewBag.IsCandidateActivated = user.Identity.IsAuthenticated && user.IsInRole(UserRoleNames.Activated);
            }
        }

        protected Guid? GetCandidateId()
        {
            Guid? candidateId = null;

            if (Request.IsAuthenticated && UserContext != null)
            {
                candidateId = UserContext.CandidateId;
            }

            return candidateId;
        }
    }
}
