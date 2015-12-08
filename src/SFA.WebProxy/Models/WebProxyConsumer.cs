namespace SFA.WebProxy.Models
{
    using System;

    public class WebProxyConsumer
    {
        public static readonly WebProxyConsumer WebProxyConsumerNotFound = new WebProxyConsumer();

        public int WebProxyConsumerId { get; set; }
        public Guid ExternalSystemId { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string RouteToCompatabilityWebServiceRegex { get; set; }
    }
}