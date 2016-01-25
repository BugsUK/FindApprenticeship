namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Address.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Address.ValidationSource")]
    public class ValidationSource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ValidationSource()
        {
            PostalAddresses = new HashSet<PostalAddress>();
        }

        [Key]
        [StringLength(3)]
        public string ValidationSourceCode { get; set; }

        [Required]
        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostalAddress> PostalAddresses { get; set; }
    }
}
