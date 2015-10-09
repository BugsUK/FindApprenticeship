namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices.Models
{
    public class ProviderSite
    {
        public int UKPRN { get; set; }
        public int ProviderSiteID { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
        public int EDSURN { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Town { get; set; }
        public int CountyId { get; set; }
        public string PostCode { get; set; }
        public int LocalAuthorityId { get; set; }
        public int ManagingAreaID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int GeocodeEasting { get; set; }
        public int GeocodeNorthing { get; set; }
        public string OwnerOrganisation { get; set; }
        public string EmployerDescription { get; set; }
        public string CandidateDescription { get; set; }
        public string WebPage { get; set; }
        public bool OutofDate { get; set; }
        public string ContactDetailsForEmployer { get; set; }
        public string ContactDetailsForCandidate { get; set; }
        public bool HideFromSearch { get; set; }
        public int TrainingProviderStatusTypeId { get; set; }
        public bool IsRecruitmentAgency { get; set; }
        public string ContactDetailsAsARecruitmentAgency { get; set; }
    }
}