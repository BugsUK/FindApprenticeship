namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Attributes;
    using Common.Constants;
    using NLog.Contrib;
    using SFA.Infrastructure.Interfaces;

    [AuthorizationData,
    OutputCache(CacheProfile = CacheProfiles.None)]
    public abstract class RecruitmentControllerBase : Common.Controllers.ControllerBase
    {
        protected RecruitmentControllerBase(ILogService loggingService) : base(loggingService)
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetPersistentLoggingInfo();
            MappedDiagnosticsLogicalContext.Set("userId", User.Identity.Name);

            LogOnActionExecuting(filterContext);

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
