namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReportDefinition
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleTypeID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [StringLength(100)]
        public string HTMLVersion { get; set; }

        [StringLength(100)]
        public string CSVVersion { get; set; }

        [StringLength(100)]
        public string SummaryVersion { get; set; }

        public string Description { get; set; }

        [StringLength(100)]
        public string GeographicSectionName { get; set; }

        [StringLength(100)]
        public string DateSectionName { get; set; }

        [StringLength(100)]
        public string ApplicationSectionName { get; set; }

        [StringLength(255)]
        public string Flags { get; set; }

        public virtual RoleType RoleType { get; set; }
    }
}
