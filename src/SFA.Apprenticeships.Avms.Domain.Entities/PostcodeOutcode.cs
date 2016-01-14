namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PostcodeOutcode")]
    public partial class PostcodeOutcode
    {
        public int PostcodeOutcodeId { get; set; }

        [Required]
        [StringLength(4)]
        public string Outcode { get; set; }
    }
}
