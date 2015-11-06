namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship
{
    public class ApprenticeshipVacancy : Vacancy
    {
        public TrainingType TrainingType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public string FrameworkCodeName { get; set; }

        public int? StandardId { get; set; }

        public ProviderVacancyStatuses Status { get; set; }
    }
}
