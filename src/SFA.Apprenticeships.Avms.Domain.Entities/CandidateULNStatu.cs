namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CandidateULNStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CandidateULNStatu()
        {
            Candidates = new HashSet<Candidate>();
        }

        [Key]
        public int CandidateULNStatusId { get; set; }

        [Required]
        [StringLength(3)]
        public string Codename { get; set; }

        [Required]
        [StringLength(10)]
        public string Shortname { get; set; }

        [Required]
        [StringLength(100)]
        public string Fullname { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
