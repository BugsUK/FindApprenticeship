namespace SFA.Apprenticeships.Infrastructure.Postcode.Strategies
{
    using Domain.Entities.Raa.Locations;

    public interface IGetValidatedGeocodedPostalAddressStrategy
    {
        PostalAddress GetValidatedPostalAddresses(string addressLine1, string postcode);
    }
}