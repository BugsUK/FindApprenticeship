namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    public class InformationRadiatorViewModel
    {
        public int TotalProviders { get; set; }
        public int TotalProvidersAskedToOnboard { get; set; }
        public int TotalProvidersForcedToMigrate { get; set; }
        public int TotalProvidersOnboarded { get; set; }
        public int TotalProvidersMigrated { get; set; }
        public int TotalProviderUserAccounts { get; set; }
        public int TotalVacanciesCreatedViaRaa { get; set; }
        public int TotalDraftVacanciesCreatedViaRaa { get; set; }
        public int TotalVacanciesInReviewViaRaa { get; set; }
        public int TotalVacanciesReferredViaRaa { get; set; }
        public int TotalVacanciesApprovedViaRaa { get; set; }
        public int TotalVacanciesClosedViaRaa { get; set; }
        public int TotalVacanciesArchivedViaRaa { get; set; }
        public int TotalApplicationsStartedForRaaVacancies { get; set; }
        public int TotalApplicationsSubmittedForRaaVacancies { get; set; }
        public int TotalUnsuccessfulApplicationsViaRaa { get; set; }
        public int TotalSuccessfulApplicationsViaRaa { get; set; }
    }
}