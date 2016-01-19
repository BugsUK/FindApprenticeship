namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DisplayText")]
    public partial class DisplayText
    {
        public int DisplayTextId { get; set; }

        [Required]
        [StringLength(250)]
        public string Type { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string StandardText { get; set; }
    }
}
