namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    public class ApprenticeshipSummary : VacancySummary
    {
        public ApprenticeshipSummary()
        {
        }
        
        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public Wage WageObject { get; set; }

        public string Wage { get; set; }

        public string WorkingWeek { get; set; }
    }
}