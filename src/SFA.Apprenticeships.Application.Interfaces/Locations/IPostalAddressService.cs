namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using Domain.Entities.Raa.Locations;

    public interface IPostalAddressService
    {
        PostalAddress GetPostalAddresses(string companyName, string primaryAddressableObject, string secondaryAddressableObject, string street, string town, string postcode);
    }
}