namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Constants;
    using Constants;
    using Raa.Common.ViewModels.Vacancy;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    [OutputCache(CacheProfile = CacheProfiles.None)]
    public abstract class ManagementControllerBase : Common.Controllers.ControllerBase
    {
        protected ManagementControllerBase(IConfigurationService configurationService, ILogService loggingService) : base(configurationService, loggingService)
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
