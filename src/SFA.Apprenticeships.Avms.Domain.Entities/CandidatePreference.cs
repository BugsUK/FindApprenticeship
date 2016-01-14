namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CandidatePreference
    {
        public int CandidatePreferenceId { get; set; }

        public int CandidateId { get; set; }

        public int? FirstFrameworkId { get; set; }

        public int? FirstOccupationId { get; set; }

        public int? SecondFrameworkId { get; set; }

        public int? SecondOccupationId { get; set; }

        public virtual ApprenticeshipFramework ApprenticeshipFramework { get; set; }

        public virtual ApprenticeshipFramework ApprenticeshipFramework1 { get; set; }

        public virtual ApprenticeshipOccupation ApprenticeshipOccupation { get; set; }

        public virtual ApprenticeshipOccupation ApprenticeshipOccupation1 { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}
