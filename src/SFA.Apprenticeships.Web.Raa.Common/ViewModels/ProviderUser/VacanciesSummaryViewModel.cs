namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    using Vacancy;
    using Web.Common.ViewModels;

    public class VacanciesSummaryViewModel
    {
        public VacanciesSummarySearchViewModel VacanciesSummarySearch { get; set; }
        public int LiveCount { get; set; }
        public int SubmittedCount { get; set; }
        public int RejectedCount { get; set; }
        public int ClosingSoonCount { get; set; }
        public int ClosedCount { get; set; }
        public int DraftCount { get; set; }
        public int NewApplicationsAcrossAllVacanciesCount { get; set; }
        public int WithdrawnCount { get; set; }
        public int CompletedCount { get; set; }
        public bool HasVacancies { get; set; }
        public PageableViewModel<VacancySummaryViewModel> Vacancies { get; set; }
    }
}