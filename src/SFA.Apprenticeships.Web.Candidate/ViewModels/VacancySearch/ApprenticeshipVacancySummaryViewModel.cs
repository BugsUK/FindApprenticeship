namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using Common.ViewModels;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;

    public class ApprenticeshipVacancySummaryViewModel : VacancySummaryViewModel
    {
        public VacancyLocationType VacancyLocationType { get; set; }
        
        public ApplicationStatuses? CandidateApplicationStatus { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public WageViewModel Wage { get; set; }

        public string WorkingWeek { get; set; }
    }
}