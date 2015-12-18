namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Person")]
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            Candidates = new HashSet<Candidate>();
            EmployerContacts = new HashSet<EmployerContact>();
            StakeHolders = new HashSet<StakeHolder>();
        }

        public int PersonId { get; set; }

        public int Title { get; set; }

        [StringLength(10)]
        public string OtherTitle { get; set; }

        [StringLength(35)]
        public string FirstName { get; set; }

        [StringLength(35)]
        public string MiddleNames { get; set; }

        [Required]
        [StringLength(35)]
        public string Surname { get; set; }

        [StringLength(16)]
        public string LandlineNumber { get; set; }

        [StringLength(16)]
        public string MobileNumber { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public int PersonTypeId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidate> Candidates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployerContact> EmployerContacts { get; set; }

        public virtual PersonTitleType PersonTitleType { get; set; }

        public virtual PersonType PersonType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StakeHolder> StakeHolders { get; set; }
    }
}
