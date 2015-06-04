namespace SFA.Apprenticeships.Infrastructure.Address.Configuration
{
    public class AddressSearchConfiguration
    {
        public string Provider { get; set; }
        public OrdnanceSurveyPlacesConfiguration OrdnanceSurveyPlacesConfiguration { get; set; }
    }

    public class OrdnanceSurveyPlacesConfiguration
    {
        public string BaseUrl { get; set; }
        public string Dataset { get; set; }
        public string ApiKey { get; set; }
    }
}