namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies
{
    using System;
    using Providers;

    public abstract class Vacancy : BaseEntity
    {
        public long VacancyReferenceNumber { get; set; }
        public string Ukprn { get; set; }
        public string Title { get; set; }
        public string TitleComment { get; set; }
        public string ShortDescription { get; set; }
        public string ShortDescriptionComment { get; set; }
        public string WorkingWeek { get; set; }
        public decimal? HoursPerWeek { get; set; }
        public WageType WageType { get; set; }
        public decimal? Wage { get; set; }
        public WageUnit WageUnit { get; set; }
        public DurationType DurationType { get; set; }
        public int? Duration { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? InterviewStartDate { get; set; }
        public DateTime? PossibleStartDate { get; set; }
        public string LongDescription { get; set; }
        public string DesiredSkills { get; set; }
        public string DesiredSkillsComment { get; set; }
        public string FutureProspects { get; set; }
        public string FutureProspectsComment { get; set; }
        public string PersonalQualities { get; set; }
        public string PersonalQualitiesComment { get; set; }
        public string ThingsToConsider { get; set; }
        public string ThingsToConsiderComment { get; set; }
        public string DesiredQualifications { get; set; }
        public string DesiredQualificationsComment { get; set; }
        public string FirstQuestion { get; set; }
        public string SecondQuestion { get; set; }
        public ProviderSiteEmployerLink ProviderSiteEmployerLink { get; set; }
        public bool OfflineVacancy { get; set; }
        public string OfflineApplicationUrl { get; set; }
        public string OfflineApplicationUrlComment { get; set; }
        public string OfflineApplicationInstructions { get; set; }
        public string OfflineApplicationInstructionsComment { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateStartedToQA { get; set; }
        public string QAUserName { get; set; }
        public DateTime? DateQAApproved { get; set; }
    }
}
