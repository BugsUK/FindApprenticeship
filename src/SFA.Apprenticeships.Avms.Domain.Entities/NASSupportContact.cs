namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NASSupportContact")]
    public partial class NASSupportContact
    {
        public int NASSupportContactId { get; set; }

        public int ManagingAreaID { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailAddress { get; set; }
    }
}
