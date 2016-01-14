namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UniqueKeyRegister")]
    public partial class UniqueKeyRegister
    {
        public int UniqueKeyRegisterId { get; set; }

        [Required]
        [StringLength(2)]
        public string KeyType { get; set; }

        [Required]
        [StringLength(30)]
        public string KeyValue { get; set; }

        public DateTime? DateTimeStamp { get; set; }
    }
}
