namespace SFA.Apprenticeships.Infrastructure.Postcode.Strategies
{
    using Domain.Entities.Raa.Locations;

    public interface IPostalAddressStrategy
    {
        PostalAddress GetPostalAddresses(string companyName, string street, string addressLine1, string addressLine2, string addressLine3, string addressLine4, string town, string postcode);
    }
}