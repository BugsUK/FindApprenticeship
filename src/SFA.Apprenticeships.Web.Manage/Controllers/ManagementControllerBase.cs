namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Common.Constants;
    using Constants;
    using NLog.Contrib;
    using Raa.Common.ViewModels.Vacancy;

    [OutputCache(CacheProfile = CacheProfiles.None)]
    public abstract class ManagementControllerBase : Common.Controllers.ControllerBase
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

        protected void SetLinks(VacancyViewModel vacancyViewModel)
        {
            vacancyViewModel.BasicDetailsLink = Url.RouteUrl(ManagementRouteNames.BasicDetails,
                new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.TrainingDetailsLink = Url.RouteUrl(ManagementRouteNames.TrainingDetails,
                new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.SummaryLink = Url.RouteUrl(ManagementRouteNames.Summary,
                new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.RequirementsProspectsLink = Url.RouteUrl(ManagementRouteNames.RequirementsAndProspoects,
                new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.QuestionsLink = Url.RouteUrl(ManagementRouteNames.Questions,
                new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.EmployerLink = Url.RouteUrl(ManagementRouteNames.EmployerInformation,
                new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
            vacancyViewModel.LocationsLink = Url.RouteUrl(ManagementRouteNames.AddLocations,
                new { vacancyReferenceNumber = vacancyViewModel.VacancyReferenceNumber });
        }
    }
}
