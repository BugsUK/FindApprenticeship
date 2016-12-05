namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    public class ApprenticeshipSummary : VacancySummary
    {
        public ApprenticeshipSummary()
        {
        }
        
        public VacancyLocationType VacancyLocationType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public Wage Wage { get; set; }

        public string WorkingWeek { get; set; }
    }
}