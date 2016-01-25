namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CAFField
    {
        [Key]
        public int CAFFieldsId { get; set; }

        public int CandidateId { get; set; }

        public int? ApplicationId { get; set; }

        public short Field { get; set; }

        [StringLength(4000)]
        public string Value { get; set; }

        public virtual Application Application { get; set; }

        public virtual CAFFieldsFieldType CAFFieldsFieldType { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}
