namespace SFA.Apprenticeships.Application.Location.Strategies
{
    using Domain.Entities.Raa.Locations;

    public interface IPostalAddressStrategy
    {
        PostalAddress GetPostalAddresses(string companyName, string primaryAddressableObject, string secondaryAddressableObject, string street, string town, string postcode);
    }
}