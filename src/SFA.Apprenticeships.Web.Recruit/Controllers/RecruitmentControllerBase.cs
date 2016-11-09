namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System.Web.Mvc;
    using Application.Interfaces;
    using Common.Attributes;
    using Common.Constants;

    [ApplyAnalytics("RecruitWebConfiguration"),
    AuthorizationData,
    Raa.Common.Attributes.CookiesEnabled,
    OutputCache(CacheProfile = CacheProfiles.None)]
    public abstract class RecruitmentControllerBase : Common.Controllers.ControllerBase
    {
        protected RecruitmentControllerBase(IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetPersistentLoggingInfo();
            SetLoggingInfo("userId", () => User.Identity.Name);

            LogOnActionExecuting(filterContext);

            SetAbout();

            base.OnActionExecuting(filterContext);
        }

        protected string GetProviderUserName()
        {            
            var userName = string.Empty;
            if (Request.IsAuthenticated)
            {                
                userName = User.Identity.Name;
            }
            return userName;
        }
    }
}
