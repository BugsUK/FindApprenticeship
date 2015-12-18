namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WageType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WageTypeID { get; set; }

        [Required]
        [StringLength(20)]
        public string WageTypeName { get; set; }
    }
}
