namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmployerSICCode
    {
        [Key]
        public int EmployerSICCodes { get; set; }

        public int EmployerId { get; set; }

        public int SICId { get; set; }

        public virtual Employer Employer { get; set; }

        public virtual SICCode SICCode { get; set; }
    }
}
