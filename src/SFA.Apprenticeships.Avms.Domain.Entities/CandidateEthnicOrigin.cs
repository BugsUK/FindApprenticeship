namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CandidateEthnicOrigin")]
    public partial class CandidateEthnicOrigin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CandidateEthnicOrigin()
        {
            Candidates = new HashSet<Candidate>();
        }

        public int CandidateEthnicOriginId { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeName { get; set; }

        [Required]
        [StringLength(100)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}