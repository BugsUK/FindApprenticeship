namespace SFA.Apprenticeships.NewDB.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WebProxy.WebProxyConsumer")]
    public partial class WebProxyConsumer
    {
        public int WebProxyConsumerId { get; set; }

        public Guid ExternalSystemId { get; set; }

        [Required]
        [StringLength(50)]
        public string ShortDescription { get; set; }

        [Required]
        public string FullDescription { get; set; }

        [Required]
        public string RouteToCompatabilityWebServiceRegex { get; set; }
    }
}
