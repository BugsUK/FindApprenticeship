namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using Domain.Entities.Raa.Locations;

    public interface IPostalAddressDetailsService
    {
        PostalAddress RetrieveValidatedAddress(string addressId);
    }
}
