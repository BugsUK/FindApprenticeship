namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdditionalAnswer")]
    public partial class AdditionalAnswer
    {
        public int AdditionalAnswerId { get; set; }

        public int ApplicationId { get; set; }

        public int AdditionalQuestionId { get; set; }

        [Required]
        [StringLength(4000)]
        public string Answer { get; set; }

        public virtual AdditionalQuestion AdditionalQuestion { get; set; }

        public virtual Application Application { get; set; }
    }
}
