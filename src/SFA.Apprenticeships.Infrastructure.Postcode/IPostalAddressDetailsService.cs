namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using Domain.Entities.Locations;

    public interface IPostalAddressDetailsService
    {
        PostalAddress RetrieveAddress(string addressId);
    }
}
