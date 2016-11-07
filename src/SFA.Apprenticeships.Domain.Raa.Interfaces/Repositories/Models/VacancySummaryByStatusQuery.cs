namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories.Models
{
    using Entities.Raa.Reference;
    using Entities.Raa.Vacancies;

    public class VacancySummaryByStatusQuery
    {
        public int PageSize { get; set; }
        public int RequestedPage { get; set; }
        public VacanciesSummaryFilterTypes Filter { get; set; }
        public VacancySummaryOrderByColumn OrderByField { get; set; }
        public string SearchString { get; set; }
        public ManageVacancySearchMode SearchMode { get; set; }
        public VacancyStatus[] DesiredStatuses { get; set; }
        public Order Order { get; set; }
        public RegionalTeam RegionalTeamName { get; set; }
    }
}
