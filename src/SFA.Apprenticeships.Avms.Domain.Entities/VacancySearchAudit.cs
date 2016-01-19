namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VacancySearchAudit")]
    public partial class VacancySearchAudit
    {
        public int VacancySearchAuditId { get; set; }

        public int SearchType { get; set; }

        [StringLength(200)]
        public string SearchTerm { get; set; }

        public int? ApprenticeshipOccupation { get; set; }

        [StringLength(4000)]
        public string ApprenticeFrameworks { get; set; }

        public int? LocationId { get; set; }

        public int? VacancyPostedSince { get; set; }

        [StringLength(8)]
        public string PostCode { get; set; }

        public int? DistanceFromInMiles { get; set; }

        public int? DistanceFromInMeters { get; set; }

        public int? Easting { get; set; }

        public int? Northing { get; set; }

        public int? WeeklyWagesFrom { get; set; }

        public int? WeeklyWagesTo { get; set; }

        public int? PageNo { get; set; }

        public int? PageSize { get; set; }

        [StringLength(100)]
        public string SortByField { get; set; }

        [StringLength(50)]
        public string SortOrder { get; set; }

        public int? TotalVacancies { get; set; }

        public int? TotalPositions { get; set; }

        public DateTime SearchDate { get; set; }

        public int? ApprenticeshipTypeId { get; set; }
    }
}
