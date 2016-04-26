using System.Collections.Generic;

namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using Constants;

    public class ReportMenu
    {
        public Dictionary<string, string> ReportList { get; private set; }

        public ReportMenu()
        {
            ReportList = new Dictionary<string, string>
            {
                {"Live Vacancies", ManagementRouteNames.ReportVacanciesList},
                {"Successful Candidates", ManagementRouteNames.ReportSuccessfulCandidates},
                {"Unsuccessful Candidates", ManagementRouteNames.ReportUnsuccessfulCandidates},
                {"Vacancy Extensions", ManagementRouteNames.ReportVacancyExtensions}
            };
        }

    }
}