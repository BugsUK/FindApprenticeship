namespace SFA.Apprenticeships.Domain.Entities.Raa.Reporting
{
    public class InformationRadiatorData
    {
        public int TotalProviders { get; set; }
        public int TotalProviderUserAccounts { get; set; }
        public int TotalVacanciesSubmittedViaRaa { get; set; }
        public int TotalVacanciesApprovedViaRaa { get; set; }
        public int TotalApplicationsStartedForRaaVacancies { get; set; }
        public int TotalApplicationsSubmittedForRaaVacancies { get; set; }
        public int TotalUnsuccessfulApplicationsViaRaa { get; set; }
        public int TotalSuccessfulApplicationsViaRaa { get; set; }
    }
}