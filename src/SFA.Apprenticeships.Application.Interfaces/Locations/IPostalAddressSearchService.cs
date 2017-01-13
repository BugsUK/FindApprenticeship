namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;

    [Obsolete("Use IPostalAddressService")]
    public interface IPostalAddressSearchService
    {
        IEnumerable<PostalAddress> GetValidatedAddress(string fullPostcode, string addressLine1);

        IEnumerable<PostalAddress> GetValidatedAddresses(string fullPostcode);
    }
}
