namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemParameter
    {
        [Key]
        public int SystemParametersId { get; set; }

        [Required]
        [StringLength(100)]
        public string ParameterName { get; set; }

        [Required]
        [StringLength(100)]
        public string ParameterType { get; set; }

        [Required]
        [StringLength(300)]
        public string ParameterValue { get; set; }

        public bool? Editable { get; set; }

        public int? LowerLimit { get; set; }

        public int? UpperLimit { get; set; }

        [StringLength(600)]
        public string Description { get; set; }
    }
}
