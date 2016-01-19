namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using Domain.Entities.Locations;

    public interface IPostalAddressSearchService
    {
        PostalAddress GetAddress(string fullPostcode, string addressLine1);
    }
}
