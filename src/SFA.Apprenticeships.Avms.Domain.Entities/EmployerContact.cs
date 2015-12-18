namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployerContact")]
    public partial class EmployerContact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployerContact()
        {
            Employers = new HashSet<Employer>();
        }

        public int EmployerContactId { get; set; }

        public int PersonId { get; set; }

        [StringLength(50)]
        public string AddressLine1 { get; set; }

        [StringLength(50)]
        public string AddressLine2 { get; set; }

        [StringLength(50)]
        public string AddressLine3 { get; set; }

        [StringLength(50)]
        public string AddressLine4 { get; set; }

        [StringLength(50)]
        public string AddressLine5 { get; set; }

        [StringLength(50)]
        public string Town { get; set; }

        public int? CountyId { get; set; }

        [StringLength(8)]
        public string PostCode { get; set; }

        public int? LocalAuthorityId { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(16)]
        public string FaxNumber { get; set; }

        [StringLength(16)]
        public string AlternatePhoneNumber { get; set; }

        public int ContactPreferenceTypeId { get; set; }

        [StringLength(50)]
        public string Availability { get; set; }

        public virtual ContactPreferenceType ContactPreferenceType { get; set; }

        public virtual County County { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employer> Employers { get; set; }

        public virtual Person Person { get; set; }

        public virtual LocalAuthority LocalAuthority { get; set; }
    }
}
