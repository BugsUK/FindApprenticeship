namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    public class ApprenticeshipSummary : VacancySummary
    {
        public ApprenticeshipSummary()
        {
            
        }

        public ApprenticeshipSummary(ApprenticeshipSummary apprenticeshipSummary) : base(apprenticeshipSummary)
        {
            VacancyLocationType = apprenticeshipSummary.VacancyLocationType;
            ApprenticeshipLevel = apprenticeshipSummary.ApprenticeshipLevel;
        }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
    }
}