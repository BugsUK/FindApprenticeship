namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Address.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Address.PostalAddress")]
    public class PostalAddress
    {
        public int PostalAddressId { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string PostTown { get; set; }

        [Required]
        public string Postcode { get; set; }

        [StringLength(3)]
        public string ValidationSourceCode { get; set; }

        public string ValidationSourceKeyValue { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateValidated { get; set; }

        public int? Easting { get; set; }

        public int? Northing { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? CountyId { get; set; }
    }
}
