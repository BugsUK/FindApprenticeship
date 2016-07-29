namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.Provider")]
    public class Provider
    {
        [Key]
        public int ProviderId { get; set; }

        // public int Upin { get; set; }

        [Required]
        public int Ukprn { get; set; }

        public string FullName { get; set; }

        public int? ProviderToUseFAA { get; set; }

        // public string TradingName { get; set; }

        // [Required]
        // public bool IsContracted { get; set; }

        // [Column(TypeName = "datetime2")]
        // public DateTime ContractedFrom { get; set; }

        // [Column(TypeName = "datetime2")]
        // public DateTime ContractedTo { get; set; }

        // [Required]
        // public int ProviderStatusTypeId { get; set; }

        // [Required]
        // [Column(TypeName = "datetime2")]
        // public bool IsNasProvider { get; set; }

        // [Column(TypeName = "datetime2")]
        // public int OriginalUpin { get; set; }
    }
}
