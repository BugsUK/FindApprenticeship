namespace SFA.Apprenticeships.Application.Location
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;

    public interface IPostalAddressLookupProvider
    {
        IEnumerable<PostalAddress> GetValidatedPostalAddresses(string addressLine1, string postcode);

        IEnumerable<PostalAddress> GetValidatedPostalAddresses(string postcode);
    }
}
