namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System.Collections.Generic;
    using Domain.Entities.Locations;

    public interface IAddressSearchService
    {
        IEnumerable<Address> GetAddressesFor(string fullPostcode);
    }
}