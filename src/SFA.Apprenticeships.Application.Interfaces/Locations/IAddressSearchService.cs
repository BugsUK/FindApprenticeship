namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System;
    using Domain.Entities.Locations;
    using Generic;

    [Obsolete]
    public interface IAddressSearchService
    {
        Pageable<Address> GetAddressesFor(string fullPostcode, int currentPage, int pageSize);
    }
}