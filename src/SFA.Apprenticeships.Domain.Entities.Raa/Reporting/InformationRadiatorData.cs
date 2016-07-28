namespace SFA.Apprenticeships.Domain.Entities.Raa.Reporting
{
    public class InformationRadiatorData
    {
        public int TotalVacanciesSubmittedViaRaa { get; set; }
        public int TotalVacanciesApprovedViaRaa { get; set; }
        public int TotalApplicationsSubmittedForRaaVacancies { get; set; }
        public int TotalUnsuccessfulApplicationsViaRaa { get; set; }
        public int TotalSuccessfulApplicationsViaRaa { get; set; }
    }
}