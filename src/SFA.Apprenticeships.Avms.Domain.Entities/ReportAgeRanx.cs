namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReportAgeRanx
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReportAgeRangeID { get; set; }

        [Required]
        [StringLength(10)]
        public string ReportAgeRangeLabel { get; set; }

        public int MinYears { get; set; }

        public int MaxYears { get; set; }
    }
}
