namespace SFA.Apprenticeships.Infrastructure.Postcode.Strategies
{
    using Domain.Entities.Raa.Locations;

    public class GetValidatedGeocodedPostalAddressStrategy : IGetValidatedGeocodedPostalAddressStrategy
    {
        public PostalAddress GetValidatedPostalAddresses(string addressLine1, string postcode)
        {
            //Start by using PCA's interactive find
            http://services.postcodeanywhere.co.uk/PostcodeAnywhere/Interactive/Find/v1.10/json.ws?Key=AA11-AA11-AA11-AA11&SearchTerm=LL11 5HJ&PreferredLanguage=English&Filter=None&UserName=David
        }
    }
}