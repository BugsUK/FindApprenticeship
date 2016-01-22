namespace SFA.Apprenticeships.Domain.Entities.WebServices
{
    using System;
    using System.Collections.Generic;

    public class WebServiceConsumer
    {
        public WebServiceConsumer()
        {
            AllowedWebServiceCategories = new List<WebServiceCategory>();
        }

        public Guid ExternalSystemId { get; set; }

        public string PublicKey { get; set; }

        public List<WebServiceCategory> AllowedWebServiceCategories { get; set; }
    }
}
