namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.ProviderSite")]
    public class ProviderSite
    {
        [Key]
        public int ProviderSiteId { get; set; }

        public string EdsUrn { get; set; }

        public string FullName { get; set; }

        public string TradingName { get; set; }

        public string EmployerDescription { get; set; }

        public string CandidateDescription { get; set; }

        public string ContactDetailsForEmployer { get; set; }

        public string ContactDetailsForCandidate { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string Town { get; set; }

        public int CountyId { get; set; }

        public string PostCode { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public string WebPage { get; set; }

        public bool OutofDate { get; set; }

        public int TrainingProviderStatusTypeId { get; set; }

        public bool HideFromSearch { get; set; }

        public bool IsRecruitmentAgency { get; set; }
    }
}
