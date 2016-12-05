namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    public class ApprenticeshipVacancyDetail : VacancyDetail
    {
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public decimal? HoursPerWeek { get; set; }
    }
}
