namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories.Models
{
    using Entities.Raa.Vacancies;

    public class VacancySummaryQuery
    {
        public int PageSize { get; set; }
        public int RequestedPage { get; set; }
        public VacancySearchMode SearchMode { get; set; }
        public string SearchString { get; set; }
        public VacancyType VacancyType { get; set; }
        public VacanciesSummaryFilterTypes Filter { get; set; }
        public int ProviderId { get; set; }
        public int ProviderSiteId { get; set; }
        public VacancySummaryOrderByColumn OrderByField { get; set; }
        public Order Order { get; set; }
    }
}
