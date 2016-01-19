namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TermsAndCondition
    {
        [Key]
        public int TermsAndConditionsId { get; set; }

        public int UserTypeId { get; set; }

        [StringLength(200)]
        public string Fullname { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        public virtual UserType UserType { get; set; }
    }
}
