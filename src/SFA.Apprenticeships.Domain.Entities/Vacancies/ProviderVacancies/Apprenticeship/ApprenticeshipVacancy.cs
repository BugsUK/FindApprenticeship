namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship
{
    public class ApprenticeshipVacancy : Vacancy
    {
        public TrainingType TrainingType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public string ApprenticeshipLevelComment { get; set; }

        public string FrameworkCodeName { get; set; }

        public string FrameworkCodeNameComment { get; set; }

        public int? StandardId { get; set; }

        public ProviderVacancyStatuses Status { get; set; }
        public string WageComment { get; set; }
        public string ClosingDateComment { get; set; }
        public string DurationComment { get; set; }
        public string LongDescriptionComment { get; set; }
        public string PossibleStartDateComment { get; set; }
        public string WorkingWeekComment { get; set; }
        public string FirstQuestionComment { get; set; }
        public string SecondQuestionComment { get; set; }
    }
}
