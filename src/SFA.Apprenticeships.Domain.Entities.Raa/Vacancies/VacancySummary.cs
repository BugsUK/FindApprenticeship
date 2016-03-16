namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;
    using Locations;
    using Reference;

    public class VacancySummary
    {
        public int VacancyId { get; set; }
        public int VacancyReferenceNumber { get; set; }
        public Guid VacancyGuid { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string WorkingWeek { get; set; }
        public decimal? HoursPerWeek { get; set; }
        public WageType WageType { get; set; }
        public decimal? Wage { get; set; }
        public WageUnit WageUnit { get; set; }
        public DurationType DurationType { get; set; }
        public int? Duration { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? PossibleStartDate { get; set; }
        public int OwnerPartyId { get; set; }
        public bool? OfflineVacancy { get; set; }
        public int OfflineApplicationClickThroughCount { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateFirstSubmitted { get; set; }
        public DateTime? DateStartedToQA { get; set; }
        public string QAUserName { get; set; }
        public DateTime? DateQAApproved { get; set; }
        public int SubmissionCount { get; set; }
        public int? VacancyManagerId { get; set; }
        public int? ParentVacancyId { get; set; }
        public TrainingType TrainingType { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        //TODO: Use Id rather than code name
        public string FrameworkCodeName { get; set; }
        public int? StandardId { get; set; }
        public string SectorCodeName { get; set; }
        public VacancyStatus Status { get; set; }
        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }
        public int? NumberOfPositions { get; set; }
        public VacancyType VacancyType { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public PostalAddress Address { get; set; }
        public RegionalTeam RegionalTeam { get; set; }
    }
}