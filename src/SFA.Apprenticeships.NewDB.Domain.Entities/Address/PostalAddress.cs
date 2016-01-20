namespace SFA.Apprenticeships.NewDB.Domain.Entities.Address
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address.PostalAddress")]
    public partial class PostalAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PostalAddress()
        {
            //VacancyParties = new HashSet<Vacancy.VacancyParty>();
        }

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
