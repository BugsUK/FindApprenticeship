namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Address.Entities;

    [Table("Reference.LocalAuthority")]
    public class LocalAuthority
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LocalAuthorityId { get; set; }

        [Required]
        [StringLength(4)]
        public string CodeName { get; set; }

        [Required]
        public string ShortName { get; set; }

        public string FullName { get; set; }
    }
}
