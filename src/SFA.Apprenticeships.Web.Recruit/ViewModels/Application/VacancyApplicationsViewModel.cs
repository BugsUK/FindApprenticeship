namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application
{
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;

    public class VacancyApplicationsViewModel
    {
        public long VacancyReferenceNumber { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public string ShortDescription { get; set; }

        public ProviderVacancyStatuses Status { get; set; }

        public PageableViewModel<ApplicationSummaryViewModel> ApplicationSummaries { get; set; } 
    }
}