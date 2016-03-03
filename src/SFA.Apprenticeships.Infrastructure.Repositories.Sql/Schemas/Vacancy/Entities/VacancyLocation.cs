namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.VacancyLocation")]
    public class VacancyLocation
    {
        public int VacancyLocationId { get; set; }

        public int VacancyId { get; set; }

        public int? NumberOfPositions { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string DirectApplicationUrl { get; set; }

        public string Town { get; set; }

        public int? CountyId { get; set; }

        public string PostCode { get; set; }

        public int? LocalAuthorityId { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }
    }
}
