namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VacancyLocation")]
    public partial class VacancyLocation
    {
        public int VacancyLocationId { get; set; }

        public int VacancyId { get; set; }

        public short? NumberofPositions { get; set; }

        [StringLength(50)]
        public string AddressLine1 { get; set; }

        [StringLength(50)]
        public string AddressLine2 { get; set; }

        [StringLength(50)]
        public string AddressLine3 { get; set; }

        [StringLength(50)]
        public string AddressLine4 { get; set; }

        [StringLength(50)]
        public string AddressLine5 { get; set; }

        [StringLength(40)]
        public string Town { get; set; }

        public int? CountyId { get; set; }

        [StringLength(8)]
        public string PostCode { get; set; }

        public int? LocalAuthorityId { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        [StringLength(256)]
        public string EmployersWebsite { get; set; }

        public virtual County County { get; set; }

        public virtual Vacancy Vacancy { get; set; }
    }
}
