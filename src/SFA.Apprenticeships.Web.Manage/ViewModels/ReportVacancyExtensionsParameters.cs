
namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System.Web.UI.WebControls;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Vacancies;

    public class ReportVacancyExtensionsParameters : ReportParameterBase
    {
        public string Status { get; set; }

        public List<ListItem> VacancyStatuses
        {
            get
            {
                return new List<ListItem>()
                {
                    new ListItem("All", "All"),
                    new ListItem(VacancyStatus.Live.ToString(), ((int)VacancyStatus.Live).ToString()),
                    new ListItem(VacancyStatus.Closed.ToString(), ((int)VacancyStatus.Closed).ToString())
                };
            }
        }

        public int? UKPRN { get; set; }
    }
}