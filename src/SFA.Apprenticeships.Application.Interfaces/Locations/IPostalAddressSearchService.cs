namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System.Collections.Generic;
    using Domain.Entities.Locations;

    public interface IPostalAddressSearchService
    {
        IEnumerable<PostalAddress> GetValidatedAddress(string fullPostcode, string addressLine1);

        IEnumerable<PostalAddress> GetValidatedAddresses(string fullPostcode);
    }
}
