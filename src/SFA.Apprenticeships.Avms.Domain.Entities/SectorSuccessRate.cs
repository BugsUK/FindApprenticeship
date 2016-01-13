namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SectorSuccessRate
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProviderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SectorID { get; set; }

        public short PassRate { get; set; }

        public bool New { get; set; }

        public virtual ApprenticeshipOccupation ApprenticeshipOccupation { get; set; }

        public virtual Provider Provider { get; set; }
    }
}
