namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Address.Entities;

    [Table("dbo.County")]
    public class County // TODO: Move to Address schema?
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public County()
        {
            PostalAddresses = new HashSet<PostalAddress>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CountyId { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeName { get; set; }

        [Required]
        public string ShortName { get; set; }

        public string FullName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostalAddress> PostalAddresses { get; set; }
    }
}
