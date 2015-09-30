namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy
{
    using System;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class VacancyViewModel
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public long VacancyReferenceNumber { get; set; }
        public string WorkingWeek { get; set; }
        public string WeeklyWage { get; set; }
        public int Duration { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public DateTime PossibleStartDate { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public string LongDescription { get; set; }
        public string DesiredSkills { get; set; }
        public string FutureProspects { get; set; }
        public string PersonalQualities { get; set; }
        public string ThingsToConsider { get; set; }
        public string DesiredQualifications { get; set; }
        public string FirstQuestion { get; set; }
        public string SecondQuestion { get; set; }
        public EmployerViewModel Employer { get; set; }
    }
}