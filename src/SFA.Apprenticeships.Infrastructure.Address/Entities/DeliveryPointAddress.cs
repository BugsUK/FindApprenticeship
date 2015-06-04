namespace SFA.Apprenticeships.Infrastructure.Address.Entities
{
    using RestSharp.Deserializers;

    public class DeliveryPointAddress
    {
        [DeserializeAs(Name = "UPRN")]
        public string Uprn { get; set; }

        [DeserializeAs(Name = "ORGANISATION_NAME")]
        public string OrganisationName { get; set; }

        [DeserializeAs(Name = "SUB_BUILDING_NAME")]
        public string SubBuildingName { get; set; }

        [DeserializeAs(Name = "BUILDING_NUMBER")]
        public int? BuildingNumber { get; set; }

        [DeserializeAs(Name = "BUILDING_NAME")]
        public string BuildingName { get; set; }

        [DeserializeAs(Name = "THOROUGHFARE_NAME")]
        public string Street { get; set; }

        [DeserializeAs(Name = "DEPENDENT_LOCALITY")]
        public string Village { get; set; }

        [DeserializeAs(Name = "POST_TOWN")]
        public string TownOrCity { get; set; }

        [DeserializeAs(Name = "POSTCODE")]
        public string Postcode { get; set; }

        [DeserializeAs(Name = "CLASSIFICATION_CODE")]
        public string classificationCode { get; set; }
    }
}