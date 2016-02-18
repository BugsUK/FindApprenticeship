namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo.Entities
{
    using Dapper.Contrib.Extensions;

    [Table("dbo.ProviderSite")]
    public class ProviderSite
    {
        public int ProviderSiteId { get; set; }

        public string FullName { get; set; }

        public string TradingName { get; set; }

        public int EDSURN { get; set; }

        public string AddressLine1 { get; set; }

        public string AddresssLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string Town { get; set; }

        public int CountyId { get; set; }

        public string PostCode { get; set; }

        public int LocalAuthorityId { get; set; }

        public int ManagingAreadID { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int GeocodeEasting { get; set; }

        public int GeocodeNothing { get; set; }

        public string EmployerDescription { get; set; }

        public string CandidateDescription { get; set; }

        public string Webpage { get; set; }

        /// <summary>
        /// stet
        /// </summary>
        public int OutofDate { get; set; }

        public string ContactDetailsForEmployer { get; set; }

        public string ContactDetailsForCandidate { get; set; }

        public int HideFromSearch { get; set; }

        public int TrainingProviderStatusTypeId { get; set; }

        public int IsRecruitmentAgency { get; set; }

        public string ContactDetailsAsARecruitmentAgency { get; set; }

    }
}
