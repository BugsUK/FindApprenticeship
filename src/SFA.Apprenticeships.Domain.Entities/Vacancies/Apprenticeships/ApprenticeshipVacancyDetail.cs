namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    public class ApprenticeshipVacancyDetail : VacancyDetail
    {
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public ApprenticeshipLocationType ApprenticeshipLocationType { get; set; }

        public decimal? HoursPerWeek { get; set; }
    }
}
