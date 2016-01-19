namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlertPreference")]
    public partial class AlertPreference
    {
        public int AlertPreferenceId { get; set; }

        public int CandidateId { get; set; }

        public int AlertTypeId { get; set; }

        public bool SMSAlert { get; set; }

        public bool EmailAlert { get; set; }

        public virtual AlertType AlertType { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}
