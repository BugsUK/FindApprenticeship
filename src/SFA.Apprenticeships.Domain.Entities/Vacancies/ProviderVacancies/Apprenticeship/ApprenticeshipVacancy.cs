namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship
{
    using Apprenticeships;

    public class ApprenticeshipVacancy : Vacancy
    {
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public string FrameworkCodeName { get; set; }

        public string TrainingSiteErn { get; set; }
    }
}
