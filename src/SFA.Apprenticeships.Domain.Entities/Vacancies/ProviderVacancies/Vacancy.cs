namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies
{
    using System;
    using Providers;

    public abstract class Vacancy
    {
        public int VacancyId { get; set; }
        public Guid VacancyGuid { get; set; }
        public int? VacancyReferenceNumber { get; set; }
        public string Ukprn { get; set; } // TODO: do we need to store UKPRN here
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
        public int OfflineApplicationClickThroughCount { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateFirstSubmitted { get; set; }
        public DateTime? DateStartedToQA { get; set; }
        public string QAUserName { get; set; }
        public DateTime? DateQAApproved { get; set; }
        public int SubmissionCount { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        // TODO: SQL: AG: rename to VacancyManagerProviderUserId (and remove comment below).
        // Id of the Provider User who created the vacancy
        public int VacancyManagerId { get; set; }

        // TODO: SQL: AG: rename to LastEditedByProviderUserId (and remove comment below).
        // Id of the Provider User who last edited the vacancy
        public int LastEditedById { get; set; }

        public Guid? ParentVacancyId { get; set; }
    }
}
