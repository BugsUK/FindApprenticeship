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
    using SFA.Infrastructure.Interfaces;

    [OutputCache(CacheProfile = CacheProfiles.None)]
    public abstract class ManagementControllerBase : Common.Controllers.ControllerBase
    {
        protected ManagementControllerBase(ILogService loggingService) : base(loggingService)
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetPersistentLoggingInfo();
            MappedDiagnosticsLogicalContext.Set("userId", User.Identity.Name);

            LogOnActionExecuting(filterContext);

            base.OnActionExecuting(filterContext);
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
