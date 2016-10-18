namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyStatus
{
    using Application;
    using Domain.Entities.Raa.Vacancies;
    using Web.Common.ViewModels;

    public class BulkDeclineCandidatesViewModel
    {
        public int VacancyId;
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public VacancyType VacancyType { get; set; }
        public PageableViewModel<ApplicationSummaryViewModel> ApplicationSummariesViewModel { get; set; }
        public VacancyApplicationsSearchViewModel VacancyApplicationsSearch { get; set; }
        public int NewApplicationsCount { get; set; }
        public int InProgressApplicationsCount { get; set; }
        public bool CanBulkDeclineCandidates { get; set; }
    }
}