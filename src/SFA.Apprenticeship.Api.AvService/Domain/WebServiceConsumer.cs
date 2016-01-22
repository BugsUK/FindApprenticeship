namespace SFA.Apprenticeship.Api.AvService.Domain
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
