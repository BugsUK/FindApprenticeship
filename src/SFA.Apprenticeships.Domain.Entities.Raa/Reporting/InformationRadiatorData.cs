﻿namespace SFA.Apprenticeships.Domain.Entities.Raa.Reporting
{
    public class InformationRadiatorData
    {
        public int TotalProviders { get; set; }
        public int TotalProvidersMigrated { get; set; }
        public int TotalVacanciesCreatedViaRaa { get; set; }
        public int TotalVacanciesSubmittedViaRaa { get; set; }
        public int TotalVacanciesApprovedViaRaa { get; set; }
        public int TotalVacanciesReferredViaRaa { get; set; }
        public int TotalVacanciesInReviewViaRaa { get; set; }
        public int VacanciesSubmittedToday { get; set; }
        public int VacanciesSubmittedYesterday { get; set; }
        public int VacanciesSubmittedTwoDaysAgo { get; set; }
        public int VacanciesSubmittedThreeDaysAgo { get; set; }
        public int VacanciesSubmittedFourDaysAgo { get; set; }
        public int VacanciesReviewedToday { get; set; }
        public int VacanciesReviewedYesterday { get; set; }
        public int VacanciesReviewedTwoDaysAgo { get; set; }
        public int VacanciesReviewedThreeDaysAgo { get; set; }
        public int VacanciesReviewedFourDaysAgo { get; set; }
        public int TotalApplicationsStartedInPastFourWeeks { get; set; }
        public int TotalApplicationsSubmittedInPastFourWeeks { get; set; }
        public int TotalNewApplicationsInPastFourWeeks { get; set; }
        public int TotalInProgressApplicationsInPastFourWeeks { get; set; }
        public int TotalUnsuccessfulApplicationsInPastFourWeeks { get; set; }
        public int TotalSuccessfulApplicationsInPastFourWeeks { get; set; }
    }
}