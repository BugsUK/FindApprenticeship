namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    public class ApprenticeshipSummary : VacancySummary
    {
        public ApprenticeshipSummary()
        {
            // TODO: US897: AG: why default this?
            WageUnit = WageUnit.Weekly;
        }

        public ApprenticeshipSummary(ApprenticeshipSummary apprenticeshipSummary) : base(apprenticeshipSummary)
        {
            VacancyLocationType = apprenticeshipSummary.VacancyLocationType;
            ApprenticeshipLevel = apprenticeshipSummary.ApprenticeshipLevel;
            Wage = apprenticeshipSummary.Wage;
            WageUnit = apprenticeshipSummary.WageUnit;
            WorkingWeek = apprenticeshipSummary.WorkingWeek;
        }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public string Wage { get; set; }

        public WageUnit WageUnit { get; set; }

        public string WorkingWeek { get; set; }
    }
}