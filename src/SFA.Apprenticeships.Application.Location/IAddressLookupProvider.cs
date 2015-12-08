namespace SFA.Apprenticeships.Application.Location
{
    using System.Collections.Generic;
    using Domain.Entities.Locations;

    public interface IAddressLookupProvider
    {
        IEnumerable<Address> GetPossibleAddresses(string postcode);
    }
}