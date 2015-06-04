namespace SFA.Apprenticeships.Infrastructure.Address.Entities
{
    using RestSharp.Deserializers;

    public class LocalPropertyIdentifier
    {
        [DeserializeAs(Name = "UPRN")]
        public string Uprn { get; set; }

        [DeserializeAs(Name = "ORGANISATION")]
        public string OrganisationName { get; set; }

        [DeserializeAs(Name = "SAO_START_NUMBER")]
        public int? SubBuildingNumber { get; set; }

        [DeserializeAs(Name = "SAO_TEXT")]
        public string SubBuildingName { get; set; }

        [DeserializeAs(Name = "PAO_START_NUMBER")]
        public int? BuildingNumber { get; set; }

        [DeserializeAs(Name = "PAO_START_SUFFIX")]
        public string BuildingNumberSuffix { get; set; }

        [DeserializeAs(Name = "PAO_TEXT")]
        public string BuildingName { get; set; }

        [DeserializeAs(Name = "STREET_DESCRIPTION")]
        public string Street { get; set; }

        [DeserializeAs(Name = "LOCALITY_NAME")]
        public string Village { get; set; }

        [DeserializeAs(Name = "TOWN_NAME")]
        public string TownOrCity { get; set; }

        [DeserializeAs(Name = "ADMINISTRATIVE_AREA")]
        public string County { get; set; }

        [DeserializeAs(Name = "POSTCODE_LOCATOR")]
        public string Postcode { get; set; }

        [DeserializeAs(Name = "CLASSIFICATION_CODE")]
        public string ClassificationCode { get; set; }

        [DeserializeAs(Name = "POSTAL_ADDRESS_CODE")]
        public string PostalAddressCode { get; set; }
    }
}