namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AttachedDocument")]
    public partial class AttachedDocument
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AttachedDocument()
        {
            Applications = new HashSet<Application>();
            VacancyOwnerRelationships = new HashSet<VacancyOwnerRelationship>();
        }

        public int AttachedDocumentId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public byte[] Attachment { get; set; }

        public int MIMEType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Application> Applications { get; set; }

        public virtual MIMEType MIMEType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VacancyOwnerRelationship> VacancyOwnerRelationships { get; set; }
    }
}
