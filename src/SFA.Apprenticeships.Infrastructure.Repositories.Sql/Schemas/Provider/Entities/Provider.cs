namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Provider.Provider")]
    public class Provider
    {
        public int ProviderId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DateUpdated { get; set; }

        [Required]
        public int UKPrn { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}
