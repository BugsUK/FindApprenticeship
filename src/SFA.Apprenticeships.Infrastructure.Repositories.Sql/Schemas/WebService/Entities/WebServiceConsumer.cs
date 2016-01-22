namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.WebService.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Address.Entities;

    [Table("WebService.WebServiceConsumer")]
    public class WebServiceConsumer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WebServiceConsumer()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WebServiceConsumerId { get; set; }

        [Required]
        [StringLength(1)]
        public string WebServiceConsumerTypeCode { get; set; }

    }
}
