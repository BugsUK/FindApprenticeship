namespace SFA.Apprenticeships.Web.Recruit.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Attributes;
    using Common.Constants;
    using NLog.Contrib;

    [AuthorizationData,
    OutputCache(CacheProfile = CacheProfiles.None)]
    public abstract class RecruitmentControllerBase : Common.Controllers.ControllerBase
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetLoggingIds();
            SetRequestInfo();

            base.OnActionExecuting(filterContext);
        }

        private void SetLoggingIds()
        {
            var sessionId = UserData.Get(UserDataItemNames.LoggingSessionId);
            if (sessionId == null)
            {
                sessionId = Guid.NewGuid().ToString("N");
                UserData.Push(UserDataItemNames.LoggingSessionId, sessionId);
            }

            MappedDiagnosticsLogicalContext.Set("sessionId", sessionId);
            MappedDiagnosticsLogicalContext.Set("userId", User.Identity.Name);
        }

        private void SetRequestInfo()
        {
            MappedDiagnosticsLogicalContext.Set("UserAgent", Request.UserAgent);
            MappedDiagnosticsLogicalContext.Set("UrlReferrer", Request.UrlReferrer == null ? "<unknown>" : Request.UrlReferrer.ToString());
            MappedDiagnosticsLogicalContext.Set("UserLanguages", Request.UserLanguages == null ? "<unknown>" : string.Join(",", Request.UserLanguages));
            MappedDiagnosticsLogicalContext.Set("CurrentCulture", CultureInfo.CurrentCulture.ToString());
            MappedDiagnosticsLogicalContext.Set("CurrentUICulture", CultureInfo.CurrentUICulture.ToString());
            var headers = Request.Headers.AllKeys.Select(key => string.Format("{0}={1}", key, Request.Headers[key]));
            MappedDiagnosticsLogicalContext.Set("Headers", string.Join(",", headers));
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
