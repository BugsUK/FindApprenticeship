namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using Common.ViewModels;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class ApprenticeshipVacancySummaryViewModel : VacancySummaryViewModel
    {
        public ApprenticeshipLocationType VacancyLocationType { get; set; }
        
        public ApplicationStatuses? CandidateApplicationStatus { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public WageViewModel WageObject { get; set; }

        public string WorkingWeek { get; set; }
    }
}