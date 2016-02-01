using SFA.Apprenticeships.Domain.Entities.Locations;

namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    public interface IRetrieveAddressService
    {
        PostalAddress RetrieveAddress(string addressId);
    }
}