namespace SFA.Apprenticeships.Domain.Entities.WebServices
{
    using System;

    public class WebServiceConsumer
    {
        public int WebServiceConsumerId { get; set; }

        public WebServiceConsumerType WebServiceConsumerType { get; set; }

        public Guid ExternalSystemId { get; set; }

        public string ExternalSystemName { get; set; }

        public string ExternalSystemPassword { get; set; }

        public bool AllowReferenceDataService { get; set; }

        public bool AllowVacancyUploadService { get; set; }

        public bool AllowVacancySummaryService { get; set; }

        public bool AllowVacancyDetailService { get; set; }
    }
}
