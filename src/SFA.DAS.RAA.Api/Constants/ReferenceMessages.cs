namespace SFA.DAS.RAA.Api.Constants
{
    public class ReferenceMessages
    {
        public const string MissingCountyIdentifier = "Please specify either a countyId or a countyCode.";
        public const string CountyNotFound = "The requested county has not been found.";

        public const string MissingLocalAuthorityIdentifier = "Please specify either a localAuthorityId or a localAuthorityCode.";
        public const string LocalAuthorityNotFound = "The requested localAuthority has not been found.";

        public const string MissingRegionIdentifier = "Please specify either a regionId or a regionCode.";
        public const string RegionNotFound = "The requested region has not been found.";
    }
}