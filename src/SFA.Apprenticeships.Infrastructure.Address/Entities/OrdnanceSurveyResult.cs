namespace SFA.Apprenticeships.Infrastructure.Address.Entities
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