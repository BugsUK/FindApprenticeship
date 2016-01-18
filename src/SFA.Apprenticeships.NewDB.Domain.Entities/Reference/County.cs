namespace SFA.Apprenticeships.NewDB.Domain.Entities.Reference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reference.County")]
    public partial class County // TODO: Move to Address schema?
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public County()
        {
            PostalAddresses = new HashSet<Address.PostalAddress>();
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
        public virtual ICollection<Address.PostalAddress> PostalAddresses { get; set; }
    }
}
