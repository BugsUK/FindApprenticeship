namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.WebService.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WebService.WebServiceConsumer")]
    public class WebServiceConsumer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WebServiceConsumerId { get; set; }

        [Required]
        [StringLength(1)]
        public string WebServiceConsumerTypeCode { get; set; }

        [Required]
        public Guid ExternalSystemId { get; set; }

        [Required]
        public string ExternalSystemName { get; set; }

        [Required]
        public string ExternalSystemPassword { get; set; }

        [Required]
        public bool AllowVacancySummaryService { get; set; }

        [Required]
        public bool AllowVacancyDetailService { get; set; }

        [Required]
        public bool AllowReferenceDataService { get; set; }

        [Required]
        public bool AllowVacancyUploadService { get; set; }
    }
}
