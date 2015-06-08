namespace SFA.Apprenticeships.Application.Services.LocationSearchService.Entities
{
    using RestSharp.Deserializers;

    public class OrdnanceSurveyResult
    {
        [DeserializeAs(Name = "DPA")]
        public DeliveryPointAddress DeliveryPointAddress { get; set; }

        [DeserializeAs(Name = "LPI")]
        public LocalPropertyIdentifier LocalPropertyIdentifier { get; set; }
    }
}